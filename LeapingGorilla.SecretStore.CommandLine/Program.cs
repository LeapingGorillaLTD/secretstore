using System;
using System.Security;
using Microsoft.Extensions.CommandLineUtils;

namespace LeapingGorilla.SecretStore.CommandLine
{
    class Program
    {
	    

        static void Main(string[] args)
        {
	        var c = new Exec();
			c.Configure();
			c.Run(args);
        }
	}

	public class Exec
	{
		private const string DefaultTableName = "SecretStore";
		private const string HelpPattern = " -? | -h | --help";

		public delegate void CreateTable(string tableName);
		public delegate void AddSecret(string key, string application, string name, SecureString value, string tableName = DefaultTableName);

		public event CreateTable OnCreateTable;
		public event AddSecret OnAddSecret;

		CommandLineApplication cml = new CommandLineApplication();
		

		public void Configure()
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
						OnCreateTable?.Invoke(table);

						return 0;
					});
				});
			
			cml.Command("add",
				(cmd) =>
				{
					var key = cmd.Argument("key", "The key to use to protect the secret.");
					var application = cmd.Argument("application", "The application that the secret belongs to.");
					var name = cmd.Argument("name", "The name of the secret to be created.");
					var tableName = cmd.Argument("[tablename]", $"The name of the table to be created (OPTIONAL - Default: {DefaultTableName}).");
					
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

						var table = tableName.ValueOrDefault(DefaultTableName);

						Console.WriteLine("Enter secret: ");
						var pw = GetSecret();
						if (pw.Length <= 0)
						{
							Console.WriteLine("You must provide the value for the secret");
							return -4;
						}
						
						Console.WriteLine("Adding secret. Key: {0}, Application: {1}, Name: {2}, Table: {3}", key.Value, application.Value, name.Value, table);
						OnAddSecret?.Invoke(key.Value, application.Value, name.Value, pw, table);
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

		public SecureString GetSecret()
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
