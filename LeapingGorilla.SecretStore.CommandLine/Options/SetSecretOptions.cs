using CommandLine;

namespace LeapingGorilla.SecretStore.CommandLine.Options
{
	[Verb("set", HelpText = "Add or update a secret in a Secret Store")]
	public class SetSecretOptions
	{
		[Option('t', "tableName", Required = true,
			HelpText = "Name of the Dynamo table containing the secret")]
		public string TableName { get; set; }
		
		[Option('a', "applicationName", Required = true,
			HelpText = "Name of the application for the secret (like YourApp.Api)")]
		public string ApplicationName { get; set; }

		[Option('s', "secretName", Required = true,
			HelpText = "Name of the secret")]
		public string SecretName { get; set; }
		
		[Option('k', "key", Required = true,
			HelpText = "ID of the key used to protect the secret. This should be the ARN of a KMS key or its alias in the format 'alias/YourKeyName'")]
		public string Key { get; set; }
	}
}
