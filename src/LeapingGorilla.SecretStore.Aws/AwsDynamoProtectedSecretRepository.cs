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
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using LeapingGorilla.SecretStore.Aws.Exceptions;
using LeapingGorilla.SecretStore.Exceptions;
using LeapingGorilla.SecretStore.Interfaces;
using Polly;

namespace LeapingGorilla.SecretStore.Aws
{
	///<summary>This class is thread safe and should be instantiated as a Singleton.</summary>
	public class AwsDynamoProtectedSecretRepository : IProtectedSecretRepository, ICreateProtectedSecretTable, IDisposable
	{
		private string _tableName;
		private readonly Lazy<Table> _table;
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

		private AwsDynamoProtectedSecretRepository(string tableName)
		{
			_tableName = tableName;
			_table = new Lazy<Table>(Init, LazyThreadSafetyMode.ExecutionAndPublication);
		}
		
		public AwsDynamoProtectedSecretRepository(AmazonDynamoDBConfig config, string tableName)
			: this(tableName)
		{
			_client = config == null ? new AmazonDynamoDBClient() : new AmazonDynamoDBClient(config);
		}
		
		public AwsDynamoProtectedSecretRepository(IAmazonDynamoDB client, string tableName)
			: this(tableName)
		{
			_client = client;
		}

		private Table Init()
		{
			Table table = null;
			var loadTableSuccess = 
				Policy
					.HandleResult(false)
					.WaitAndRetry(3, (retryCount, ctx) => TimeSpan.FromSeconds(retryCount * 2))
					.Execute(() => Table.TryLoadTable(_client, _tableName, out table));


			if (!loadTableSuccess)
			{
				throw new DynamoTableDoesNotExistException(_tableName);
			}

			return table;
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

			// ReSharper disable once UnusedVariable - Used to test for existence
			if (!Table.TryLoadTable(_client, _tableName, out var table))
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
		public async Task<ProtectedSecret> GetAsync(string applicationName, string secretName)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException("AwsDynamoProtectedSecretRepository");
			}

			var document = await _table.Value.GetItemAsync(applicationName, secretName).ConfigureAwait(false);
			if (document == null)
			{
				throw new SecretNotFoundException(applicationName, secretName);
			}

			return document.ToProtectedSecret();
		}

		/// <inheritdoc />
		public IEnumerable<ProtectedSecret> GetAllForApplication(string applicationName)
		{
			var t = GetAllForApplicationAsync(applicationName);
			t.ConfigureAwait(false);
			return t.GetAwaiter().GetResult();
		}
		
		/// <inheritdoc />
		public async Task<IEnumerable<ProtectedSecret>> GetAllForApplicationAsync(string applicationName)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException("AwsDynamoProtectedSecretRepository");
			}

			var filter = new ScanFilter();
			filter.AddCondition(Fields.ApplicationName, ScanOperator.Equal, applicationName);
			
			var search = _table.Value.Scan(filter);
			var bg = await search.GetRemainingAsync().ConfigureAwait(false);

			if (bg == null || bg.Count == 0)
			{
				return Enumerable.Empty<ProtectedSecret>();
			}

			return bg.Select(AwsExtensions.ToProtectedSecret).ToList();
		}

		/// <inheritdoc />
		public void Save(ProtectedSecret secret)
		{
			var t = SaveAsync(secret);
			t.ConfigureAwait(false);
			t.GetAwaiter().GetResult();
		}

		/// <inheritdoc />
		public async Task SaveAsync(ProtectedSecret secret)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException("AwsDynamoProtectedSecretRepository");
			}

			var doc = new Document
			{
				[Fields.ApplicationName] = secret.ApplicationName,
				[Fields.SecretName] = secret.Name,
				[Fields.MasterKeyId] = secret.MasterKeyId,
				[Fields.ProtectedDocumentKey] = secret.ProtectedDocumentKey,
				[Fields.ProtectedSecretValue] = secret.ProtectedSecretValue,
				[Fields.InitialisationVector] = secret.InitialisationVector
			};

			await _table.Value.PutItemAsync(doc).ConfigureAwait(false);
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