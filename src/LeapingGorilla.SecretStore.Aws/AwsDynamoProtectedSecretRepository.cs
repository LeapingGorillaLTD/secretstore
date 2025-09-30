// /*
//    Copyright 2013-2022 Leaping Gorilla LTD
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//        http://www.apache.org/licenses/LICENSE-2.0
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// */

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using LeapingGorilla.SecretStore.Exceptions;
using LeapingGorilla.SecretStore.Interfaces;
using Microsoft.Extensions.Logging;
using Polly;

namespace LeapingGorilla.SecretStore.Aws
{
	///<summary>This class is thread safe and should be instantiated as a Singleton.</summary>
	public class AwsDynamoProtectedSecretRepository : IProtectedSecretRepository, ICreateProtectedSecretTable, IDisposable
	{
		private readonly ILogger<AwsDynamoProtectedSecretRepository> _log;
		private string _tableName;
		private IAmazonDynamoDB _client;
		private bool _disposed;

		internal static class Fields
		{
			public const string ApplicationName = "ApplicationName";
			public const string SecretName = "SecretName";
			public const string MasterKeyId = "MasterKeyId";
			public const string ProtectedDocumentKey = "ProtectedDocumentKey";
			public const string ProtectedSecretValue = "ProtectedSecretValue";
			public const string InitialisationVector = "InitialisationVector";
		}

		private AwsDynamoProtectedSecretRepository(
			ILogger<AwsDynamoProtectedSecretRepository> log,
			string tableName)
		{
			_log = log;
			_tableName = tableName;
		}
		
		public AwsDynamoProtectedSecretRepository(
			ILogger<AwsDynamoProtectedSecretRepository> log,
			AmazonDynamoDBConfig config, 
			string tableName)
			: this(log, tableName)
		{
			_client = config == null ? new AmazonDynamoDBClient() : new AmazonDynamoDBClient(config);
		}
		
		public AwsDynamoProtectedSecretRepository(
			ILogger<AwsDynamoProtectedSecretRepository> log,
			IAmazonDynamoDB client, string tableName)
			: this(log, tableName)
		{
			_client = client;
		}

		/// <inheritdoc />
		public async Task CreateProtectedSecretTableAsync(string secretTableName)
		{
			_tableName = secretTableName;
			var tableDetail = new CreateTableRequest
			{
				TableName = _tableName,
				AttributeDefinitions = new List<AttributeDefinition>
				{
					new AttributeDefinition { AttributeName = Fields.ApplicationName, AttributeType = ScalarAttributeType.S },
					new AttributeDefinition { AttributeName = Fields.SecretName, AttributeType = ScalarAttributeType.S },
				},

				KeySchema = new List<KeySchemaElement>
				{
					new KeySchemaElement { AttributeName = Fields.ApplicationName, KeyType = KeyType.HASH },
					new KeySchemaElement { AttributeName = Fields.SecretName, KeyType = KeyType.RANGE }
				},

				ProvisionedThroughput = new ProvisionedThroughput
				{
					ReadCapacityUnits = 10,
					WriteCapacityUnits = 5
				}
			};

			bool tableExists;
			try
			{
				var describeTableResponse = await _client.DescribeTableAsync(_tableName);
				tableExists = describeTableResponse.HttpStatusCode == HttpStatusCode.OK;
			}
			catch (ResourceNotFoundException)
			{
				tableExists = false;
			}
			
			if (!tableExists)
			{
				await _client.CreateTableAsync(tableDetail);
				var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));

				await Policy
					.HandleResult<TableStatus>(ts => ts != TableStatus.ACTIVE)
					.WaitAndRetryAsync(3, retryCount => TimeSpan.FromSeconds(retryCount * 2))
					.ExecuteAsync(async ctx =>
					{
						var describeTableResponse = await _client.DescribeTableAsync(_tableName, ctx);
						return describeTableResponse.Table.TableStatus;
					}, tokenSource.Token);
			}
		}

		/// <inheritdoc />
		public ProtectedSecret Get(string applicationName, string secretName)
		{
			var t = GetAsync(applicationName, secretName);
			t.ConfigureAwait(false);
			return t.GetAwaiter().GetResult();
		}

		/// <inheritdoc />
		public async Task<ProtectedSecret> GetAsync(
			string applicationName, 
			string secretName,
			CancellationToken cancellationToken = default)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(nameof(AwsDynamoProtectedSecretRepository));
			}
			
			try
			{
				var getItemResponse = await _client.GetItemAsync(_tableName, CreateKeyDescription(applicationName, secretName), cancellationToken);
				if (getItemResponse.Item == null)
				{
					throw new SecretNotFoundException(applicationName, secretName);
				}
				return getItemResponse.Item.ToProtectedSecret();
			}
			catch (AmazonDynamoDBException ex)
			{
				_log.LogWarning(ex, "Failed to find secret {SecretName} for application {ApplicationName} in table {TableName}", secretName, applicationName, _tableName);
				throw;
			}
		}

		/// <inheritdoc />
		public IEnumerable<ProtectedSecret> GetAllForApplication(string applicationName)
		{
			var t = GetAllForApplicationAsync(applicationName);
			t.ConfigureAwait(false);
			return t.GetAwaiter().GetResult();
		}
		
		/// <inheritdoc />
		public async Task<IEnumerable<ProtectedSecret>> GetAllForApplicationAsync(
			string applicationName,
			CancellationToken cancellationToken = default)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException("AwsDynamoProtectedSecretRepository");
			}

			var scanReq = new ScanRequest(_tableName)
			{
				FilterExpression = $"{Fields.ApplicationName} = :v_appname",
				ExpressionAttributeValues = new Dictionary<string, AttributeValue>
				{
					{ ":v_appname", new AttributeValue { S = applicationName } }
				}
			};

			try
			{
				var scanResult = await _client.ScanAsync(scanReq, cancellationToken);
				if (scanResult.Items == null || scanResult.Items.Count == 0)
				{
					return Enumerable.Empty<ProtectedSecret>();
				}
				
				return scanResult.Items.Select(AwsExtensions.ToProtectedSecret).ToList();
			}
			catch (AmazonDynamoDBException ex)
			{
				_log.LogWarning(ex, "Failed to scan for secrets for application {ApplicationName} in table {TableName}", applicationName, _tableName);
				throw;
			}
		}

		/// <inheritdoc />
		public void Save(ProtectedSecret secret)
		{
			var t = SaveAsync(secret);
			t.ConfigureAwait(false);
			t.GetAwaiter().GetResult();
		}

		/// <inheritdoc />
		public async Task SaveAsync(
			ProtectedSecret secret,
			CancellationToken cancellationToken = default)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException("AwsDynamoProtectedSecretRepository");
			}

			var res = await _client.PutItemAsync(_tableName, secret.ToAttributeMap(), cancellationToken);
			if (res.HttpStatusCode != HttpStatusCode.OK)
			{
				throw new SecretStoreException($"Failed to save secret {secret.Name} for application {secret.ApplicationName} to table {_tableName}. AWS returned status code {res.HttpStatusCode}");
			}
		}
		
		
		
		/// <summary>
		/// Create a Key Descriptor for the given application and secret name
		/// </summary>
		/// <param name="applicationName">Name of the application that the secret belongs to</param>
		/// <param name="secretName">Name of the secret that we are retrieving</param>
		/// <returns>Dictionary that can be passed to a GetItem call</returns>
		private Dictionary<string, AttributeValue> CreateKeyDescription(string applicationName, string secretName)
		{
			return new Dictionary<string, AttributeValue>
			{
				{ Fields.ApplicationName, new AttributeValue { S = applicationName } },
				{ Fields.SecretName, new AttributeValue { S = secretName } }
			};
		}

		[ExcludeFromCodeCoverage]
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		[ExcludeFromCodeCoverage]
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				_client?.Dispose();
				_client = null;
			}

			_disposed = true;
		}
	}
}