using CommandLine;

namespace LeapingGorilla.SecretStore.CommandLine.Options
{
	[Verb("getAll", HelpText = "Get all secrets for an application from the Secret Store")]
	public class GetAllSecretsOption
	{
		[Option('t', "tableName", Required = true,
			HelpText = "Name of the Dynamo table that we are reading the secrets from")]
		public string TableName { get; set; }
		
		[Option('a', "applicationName", Required = true,
			HelpText = "Name of the application for the secrets (like YourApp.Api)")]
		public string ApplicationName { get; set; }
	}
}
