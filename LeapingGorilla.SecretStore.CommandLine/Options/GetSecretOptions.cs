using CommandLine;

namespace LeapingGorilla.SecretStore.CommandLine.Options
{
	[Verb("get", HelpText = "Get the value of a single secret from a secret store")]
	public class GetSecretOptions
	{
		[Option('t', "tableName", Required = true,
			HelpText = "Name of the Dynamo table that we are reading the secret from")]
		public string TableName { get; set; }
		
		[Option('a', "applicationName", Required = true,
			HelpText = "Name of the application for the secret (like YourApp.Api)")]
		public string ApplicationName { get; set; }
		
		[Option('s', "secretName", Required = true,
			HelpText = "Name of the secret to be retrieved")]
		public string Name { get; set; }
	}
}
