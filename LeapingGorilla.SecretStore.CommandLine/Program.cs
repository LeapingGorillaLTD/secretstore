using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using Amazon;
using Amazon.DynamoDBv2;
using Microsoft.Extensions.CommandLineUtils;
using LeapingGorilla.SecretStore.Aws;

namespace LeapingGorilla.SecretStore.CommandLine
{
	
    class Program
    {
		static void Main(string[] args)
		{
			var region = RegionEndpoint.EUWest1;
	        var km = new AwsKmsKeyManager(region);
	        var config = new AmazonDynamoDBConfig
	        {
		        RegionEndpoint = region
	        };
        
			var repo = new AwsDynamoProtectedSecretRepository(config, CommandLineInterface.DefaultTableName);
	        var em = new EncryptionManager(km);
	        var ss = new SecretStore(repo, em);

	        var imp = new CommandImplementation(ss, repo);

	        var c = new CommandLineInterface();
			c.Configure(imp);
		    c.Run(args);
        }
	}

	public class CommandLineInterface
	{
		public const string DefaultTableName = "SecretStore";
		private const string HelpPattern = " -? | -h | --help";

		private readonly CommandLineApplication cml = new CommandLineApplication();
		

		public void Configure(CommandImplementation imp)
		{			
			cml.Command("createTable",
				(cmd) =>
				{
					var createTable = cmd.Argument("[tablename]", $"The name of the table to be created (Default: {DefaultTableName}).");
					cmd.HelpOption(HelpPattern);
					cmd.ExtendedHelpText = $"Create a new Secret Store table with the name specified. Creates a table with the name {DefaultTableName} if none provided.";
					cmd.OnExecute(() =>
					{
						var table = String.IsNullOrWhiteSpace(createTable.Value)
									? DefaultTableName
									: createTable.Value;

						Console.WriteLine("Creating table: {0}", table);
						imp.CreateTable(table);
						Console.WriteLine("Done");

						return 0;
					});
				});
			
			cml.Command("add",
				(cmd) =>
				{
					var key = cmd.Argument("key", "The key to use to protect the secret.");
					var application = cmd.Argument("application", "The application that the secret belongs to.");
					var name = cmd.Argument("name", "The name of the secret to be created.");
					
					cmd.HelpOption(HelpPattern);
					cmd.ExtendedHelpText = "Add a new secret into the Secret Store for the given application, name and using the specified key";
					
					cmd.OnExecute(() =>
					{
						if (!key.HasValue())
						{
							cmd.ShowHelp();
							return -1;
						}

						if (!application.HasValue())
						{
							Console.WriteLine("You must provide the name of the application");
							Console.WriteLine("secrets add arn:mykey MyApplication NameOfSecret [table name]");
							return -2;
						}

						if (!name.HasValue())
						{
							Console.WriteLine("You must provide the name of the secret");
							Console.WriteLine("secrets add arn:mykey MyApplication NameOfSecret [table name]");
							return -3;
						}
						
						Console.WriteLine("Enter secret: ");
						var pw = GetSecretFromConsole();
						if (pw.Length <= 0)
						{
							Console.WriteLine("You must provide the value for the secret");
							return -4;
						}
						
						Console.WriteLine("Adding secret. Key: {0}, Application: {1}, Name: {2}", key.Value, application.Value, name.Value);
						imp.AddSecret(key.Value, application.Value, name.Value, pw);
						Console.WriteLine("Done");
						return 0;
					});
				});

			cml.Command("get",
				(cmd) =>
				{
					var application = cmd.Argument("application", "The application that the secret belongs to.");
					var name = cmd.Argument("[name]", "The name of the secret to be created. (OPTIONAL - if ommitted all secrets for the application will be retrieved");
					
					cmd.HelpOption(HelpPattern);
					cmd.ExtendedHelpText = "Add a new secret into the Secret Store for the given application, name and using the specified key";
					
					cmd.OnExecute(() =>
					{
						if (!application.HasValue())
						{
							Console.WriteLine("You must provide the name of the application");
							Console.WriteLine("secrets get MyApplication NameOfSecret [table name]");
							return -2;
						}
						

						if (name.HasValue())
						{
							var pw = imp.GetSecret(application.Value, name.Value);
							if (pw == null)
							{
								Console.WriteLine("No secret found for application {0} with name {1}", application.Value, name.Value);
								return -4;
							}
							
							Console.WriteLine("{0, -15}|{1, -15}|{2, -15}", "Application", "Name", "PW");
							Console.WriteLine("{0, -15}|{1, -15}|{2, -15}", application, name, pw);
						}
						else
						{
							var pws = imp.GetAllSecretsForApplication(application.Value);
							if (pws == null || !pws.Any())
							{
								Console.WriteLine("No secrets found for application {0}", application.Value);
								return -5;
							}
							
							Console.WriteLine("{0, -15}|{1, -15}|{2, -15}", "Application", "Name", "PW");
							foreach (var pw in pws)
							{
								Console.WriteLine("{0, -15}|{1, -15}|{2, -15}", application, name, pw);
							}
						}
						
						return 0;
					});
				});

			cml.HelpOption(HelpPattern);
			cml.OnExecute(() =>
			{
				cml.ShowHelp();
				return 0;
			});
		}
		
		public void Run(string[] args)
		{
			cml.Execute(args);
		}

		public SecureString GetSecretFromConsole()
		{
			var pwd = new SecureString();
			while (true)
			{
				var keyInfo = Console.ReadKey(true);
				if (keyInfo.Key == ConsoleKey.Enter)
				{
					break;
				}
				
				if (keyInfo.Key == ConsoleKey.Backspace && pwd.Length > 0)
				{
					pwd.RemoveAt(pwd.Length - 1);
					Console.Write("\b \b");
					continue;
				}
				
				if (!char.IsControl(keyInfo.KeyChar))
				{
					pwd.AppendChar(keyInfo.KeyChar);
					Console.Write("*");
				}
			}
			Console.WriteLine();
			return pwd;
		}
	}
}
