namespace LeapingGorilla.SecretStore.Tests.Builders
{
	public class ClearSecretBuilder
	{
		public class Defaults
		{
			public const string ApplicationName = "TestApplication";
			public const string Name = "TestSecret";
			public const string Value = "Terst Secret Value";
		}

		private readonly ClearSecret _instance;

		private ClearSecretBuilder()
		{
			_instance = new ClearSecret(Defaults.ApplicationName, Defaults.Name, Defaults.Value);
		}

		public static ClearSecretBuilder Create()
		{
			return new ClearSecretBuilder();
		}

		public ClearSecret AnInstance()
		{
			return _instance;
		}

		public ClearSecretBuilder WithApplicationName(string applicationName)
		{
			_instance.ApplicationName = applicationName;
			return this;
		}

		public ClearSecretBuilder WithName(string name)
		{
			_instance.Name = name;
			return this;
		}

		public ClearSecretBuilder WithValue(string value)
		{
			_instance.Value = value;
			return this;
		}
	}
}
