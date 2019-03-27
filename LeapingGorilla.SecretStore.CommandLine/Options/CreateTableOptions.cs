using CommandLine;

namespace LeapingGorilla.SecretStore.CommandLine.Options
{
	[Verb("createTable", HelpText = "Create a new Secret Store table in Dynamo with the name specified")]
	public class CreateTableOptions
	{
		[Option("TableName", HelpText = "Name of the table to create in Dynamo", Required = true)]
		public string TableName { get; set; }
	}
}
