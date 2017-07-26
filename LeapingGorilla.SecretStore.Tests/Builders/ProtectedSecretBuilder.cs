using System;

namespace LeapingGorilla.SecretStore.Tests.Builders
{
	public class ProtectedSecretBuilder
	{
		public class Defaults
		{
			public const string ApplicationName = "TestApplication";
			public const string Name = "TestSecret";
			public const string MasterKeyId = "TestingMasterKeyId";
			public static readonly byte[] InitialisationVector = Convert.FromBase64String("SW5pdGlhbGlzYXRpb25WZWN0b3I=");
			public static readonly byte[] ProtectedDocumentKey = Convert.FromBase64String("UHJvdGVjdGVkRG9jdW1lbnRLZXk=");
			public static readonly byte[] ProtectedSecretValue = Convert.FromBase64String("UHJvdGVjdGVkU2VjcmV0VmFsdWU=");
		}

		private ProtectedSecret _instance;

		private ProtectedSecretBuilder()
		{
			_instance = new ProtectedSecret
			{
				ApplicationName = Defaults.ApplicationName,
				Name = Defaults.Name,
				MasterKeyId = Defaults.MasterKeyId, 
				InitialisationVector = Defaults.InitialisationVector,
				ProtectedDocumentKey = Defaults.ProtectedDocumentKey,
				ProtectedSecretValue = Defaults.ProtectedSecretValue
			};
		}

		public static ProtectedSecretBuilder Create()
		{
			return new ProtectedSecretBuilder();
		}

		public ProtectedSecret AnInstance()
		{
			return _instance;
		}

		public ProtectedSecretBuilder WithApplicationName(string applicationName)
		{
			_instance.ApplicationName = applicationName;
			return this;
		}

		public ProtectedSecretBuilder WithName(string name)
		{
			_instance.Name = name;
			return this;
		}

		public ProtectedSecretBuilder WithMasterKeyId(string masterKeyId)
		{
			_instance.MasterKeyId = masterKeyId;
			return this;
		}

		public ProtectedSecretBuilder WithInitialisationVector(byte[] vector)
		{
			_instance.InitialisationVector = vector;
			return this;
		}

		public ProtectedSecretBuilder WithProtectedDocumentKey(byte[] key)
		{
			_instance.ProtectedDocumentKey = key;
			return this;
		}

		public ProtectedSecretBuilder WithProtectedSecretValue(byte[] secret)
		{
			_instance.ProtectedSecretValue = secret;
			return this;
		}
	}
}
