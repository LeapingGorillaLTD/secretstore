﻿using System;
using System.Collections.Generic;
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
	public class AwsDynamoProtectedSecretRepository : IProtectedSecretRepository, IDisposable
	{
		private readonly string _tableName;
		private Table _table;
		private AmazonDynamoDBClient _client;
		private bool _disposed;
		private Task _initialise;

		private static class Fields
		{
			public const string ApplicationName = "ApplicationName";
			public const string SecretName = "SecretName";
			public const string MasterKeyId = "MasterKeyId";
			public const string ProtectedDocumentKey = "ProtectedDocumentKey";
			public const string ProtectedSecretValue = "ProtectedSecretValue";
			public const string InitialisationVector = "InitialisationVector";
		}

		public AwsDynamoProtectedSecretRepository(AmazonDynamoDBConfig config, string tableName)
		{
			_client = new AmazonDynamoDBClient(config);
			_tableName = tableName;
			_initialise = Task.Run(() => Init());
		}

		private void Init()
		{
			var loadTableSuccess = 
				Policy
					.HandleResult(false)
					.WaitAndRetry(new[]
					{
						TimeSpan.FromSeconds(1),
						TimeSpan.FromSeconds(3),
						TimeSpan.FromSeconds(6)
					})
					.Execute(() => Table.TryLoadTable(_client, _tableName, out _table));


			if (!loadTableSuccess)
			{
				throw new DynamoTableDoesNotExistException(_tableName);
			}
		}

		public async Task CreateProtectedSecretTableAsync(string tableName)
		{
			await _initialise;
			var tableDetail = new CreateTableRequest
			{
				TableName = tableName,
				AttributeDefinitions = new List<AttributeDefinition>
				{
					new AttributeDefinition { AttributeName = Fields.ApplicationName, AttributeType = ScalarAttributeType.S },
					new AttributeDefinition { AttributeName = Fields.SecretName, AttributeType = ScalarAttributeType.S },
					new AttributeDefinition { AttributeName = Fields.MasterKeyId, AttributeType = ScalarAttributeType.S },
					new AttributeDefinition { AttributeName = Fields.ProtectedDocumentKey, AttributeType = ScalarAttributeType.B },
					new AttributeDefinition { AttributeName = Fields.ProtectedSecretValue, AttributeType = ScalarAttributeType.B },
					new AttributeDefinition { AttributeName = Fields.InitialisationVector, AttributeType = ScalarAttributeType.B }
				},

				KeySchema = new List<KeySchemaElement>
				{
					new KeySchemaElement { AttributeName = Fields.ApplicationName, KeyType = KeyType.HASH },
					new KeySchemaElement { AttributeName = Fields.SecretName, KeyType = KeyType.RANGE }
				},

				ProvisionedThroughput = new ProvisionedThroughput
				{
					ReadCapacityUnits = 5,
					WriteCapacityUnits = 5
				}
			};

			if (!Table.TryLoadTable(_client, _tableName, out _table))
			{
				await _client.CreateTableAsync(tableDetail);
				Policy
					.HandleResult(false)
					.WaitAndRetry(new[]
					{
						TimeSpan.FromSeconds(1),
						TimeSpan.FromSeconds(3),
						TimeSpan.FromSeconds(6)
					})
					.Execute(() => Table.TryLoadTable(_client, _tableName, out _table));
			}
		}

		public ProtectedSecret Get(string applicationName, string secretName)
		{
			return GetAsync(applicationName, secretName).Result;
		}

		public async Task<ProtectedSecret> GetAsync(string applicationName, string secretName)
		{
			
			if (_disposed)
			{
				throw new ObjectDisposedException("AwsDynamoProtectedSecretRepository");
			}

			var document = await _table.GetItemAsync(applicationName, secretName);
			if (document == null)
			{
				throw new SecretNotFoundException(applicationName, secretName);
			}

			return new ProtectedSecret
			{
				ApplicationName = document[Fields.ApplicationName].AsString(),
				Name = document[Fields.SecretName].AsString(),
				MasterKeyId = document[Fields.MasterKeyId].AsString(),
				ProtectedDocumentKey = document[Fields.ProtectedDocumentKey].AsByteArray(),
				ProtectedSecretValue = document[Fields.ProtectedSecretValue].AsByteArray(),
				InitialisationVector = document[Fields.InitialisationVector].AsByteArray()
			};
		}

		public void Save(ProtectedSecret secret)
		{
			SaveAsync(secret).Wait(15000);
		}

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

			await _table.PutItemAsync(doc);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				_client?.Dispose();
				_client = null;

				_initialise?.Dispose();
				_initialise = null;
			}

			_disposed = true;
		}
	}
}