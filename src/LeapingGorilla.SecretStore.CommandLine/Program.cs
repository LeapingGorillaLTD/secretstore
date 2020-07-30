using System;
using System.Linq;
using System.Security;
using CommandLine;
using LeapingGorilla.SecretStore.CommandLine.Options;

namespace LeapingGorilla.SecretStore.CommandLine
{
	class Program
	{
		static void Main(string[] args)
		{
			var imp = new CommandImplementation();
			Parser.Default
				.ParseArguments<CreateTableOptions, SetSecretOptions, GetSecretOptions, GetAllSecretsOption>(args)
					.WithParsed<CreateTableOptions>(opts => imp.CreateTable(opts.TableName))
					.WithParsed<SetSecretOptions>(opts =>
					{
						Console.WriteLine("Enter secret: ");
						var pw = GetSecretFromConsole();
						if (pw.Length <= 0)
						{
							Console.WriteLine("You must provide a value for the secret");
						}
						imp.AddSecret(opts.TableName, opts.Key, opts.ApplicationName, opts.SecretName, pw);
					})
					.WithParsed<GetSecretOptions>(opts =>
					{
						var secret = imp.GetSecret(opts.TableName, opts.ApplicationName, opts.Name);
						if (secret == null)
						{
							Console.WriteLine("No secret found");
							return;
						}

						Console.WriteLine(secret);
					})
					.WithParsed<GetAllSecretsOption>(opts =>
					{
						var secrets = (imp.GetAllSecretsForApplication(opts.TableName, opts.ApplicationName)
										?? Enumerable.Empty<ClearSecret>()).ToList();
						if (!secrets.Any())
						{
							Console.WriteLine("No Secrets found");
							return;
						}

						Console.WriteLine("┌{0, -25}┬{1,-45}┐", new string('─', 25), new string('─', 45));
						Console.WriteLine("│{0, -25}│{1,-45}│", " Name", " Value");
						
						foreach (var secret in secrets)
						{
							Console.WriteLine("├{0, -25}┼{1,-45}┤", new string('─', 25), new string('─', 45));
							int iteration = 0;
							int charsLeft = secret.Value.Length;
							while (charsLeft > 0)
							{
								Console.WriteLine("│{0, -25}│{1,-45}│", 
									iteration == 0 ? secret.Name : String.Empty, 
									secret.Value.Substring(iteration * 45, charsLeft >= 45 ? 45 : charsLeft));
								
								charsLeft -= 45;
								iteration++;
							}
						}
						Console.WriteLine("└{0, -25}┴{1,-45}┘", new string('─', 25), new string('─', 45));
					});
		}

		public static SecureString GetSecretFromConsole()
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
