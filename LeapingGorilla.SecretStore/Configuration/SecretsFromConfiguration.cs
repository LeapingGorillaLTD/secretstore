using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using LeapingGorilla.SecretStore.Exceptions;
using LeapingGorilla.SecretStore.Interfaces;

namespace LeapingGorilla.SecretStore.Configuration
{
	///<summary>
	/// Retrieves secret details using a name and key specified in a section of the configuration file. 
	/// It is recommended that you create an instance of this class as a singleton for best performance. 
	/// </summary>
	/// <remarks>
	/// Sample configuration:
	/// 
	/// &lt;?xml version="1.0" encoding="utf-8" ?&gt;
	/// &lt;configuration&gt;
	/// 	&lt;configSections&gt;
	/// 		&lt;section name="SecretStore" type="LeapingGorilla.SecretStore.Configuration.SecretConfigurationSection, LeapingGorilla.SecretStore"/&gt;
	/// 	&lt;/configSections&gt;
	/// 
	/// 	&lt;SecretStore&gt;
	/// 		&lt;Secrets&gt;
	/// 			&lt;add key="MyNameInCode" application="YourApp" name="NameOfSecretInSecretStore" /&gt;
	/// 			&lt;add key="MyOtherNameInCode" application="YourApp" name="NameOfAnotherSecretInSecretStore" /&gt;
	/// 			&lt;add key="RedisConnectionString" application="Common" name="RedisConnectionString.Dev" /&gt;
	/// 			&lt;add key="DBConnectionString" application="YourApp" name="DBConnectionString.Dev" /&gt;
	/// 		&lt;/Secrets&gt;
	/// 	&lt;/SecretStore&gt;
	/// &lt;/configuration&gt;
	/// 
	/// We have a key, name and application structure so that you can use the name in code and apply config transforms 
	/// to use different keys on different environments without a code change.
	/// </remarks>
	public class SecretsFromConfiguration : ISecrets
	{
		private readonly Dictionary<string, SecretElement> _secrets = new Dictionary<string, SecretElement>();
		private readonly ISecretStore _secretStore;

		/// <summary>
		/// Initializes a new instance of the <see cref="SecretsFromConfiguration"/> class with 
		/// default behaviour. This constructor assumes that only a single SecretConfigurationSection
		/// is specified in the application config.
		/// </summary>
		/// <param name="secretStore">The secret store.</param>
		/// <exception cref="System.Configuration.ConfigurationErrorsException"></exception>
		public SecretsFromConfiguration(ISecretStore secretStore)
		{
			_secretStore = secretStore;

			var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			var sectionNames = new HashSet<string>();

			foreach (var sec in config.Sections.OfType<SecretConfigurationSection>())
			{
				sectionNames.Add(sec.SectionInformation.Name);
			}

			if (sectionNames.Count > 1)
			{
				throw new ConfigurationErrorsException($"You may only declare a single SecretConfigurationSection (Found {sectionNames.Count} - {String.Join(", ", sectionNames)})");
			}

			if (sectionNames.Count <= 0)
			{
				throw new ConfigurationErrorsException("You must declare a SecretConfigurationSection");
			}

			LoadSecretDetailsFromConfigurationSection(sectionNames.Single());
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SecretsFromConfiguration"/> class containing 
		/// details only for secrets in a named SecretConfigurationSection.
		/// </summary>
		/// <param name="secretStore">The secret store.</param>
		/// <param name="configurationSectionName">SecretName of the configuration section.</param>
		public SecretsFromConfiguration(ISecretStore secretStore, string configurationSectionName)
		{
			_secretStore = secretStore;
			LoadSecretDetailsFromConfigurationSection(configurationSectionName);
		}

		private void LoadSecretDetailsFromConfigurationSection(string configurationSectionName)
		{
			var section = ConfigurationManager.GetSection(configurationSectionName) as SecretConfigurationSection;
			if (section == null)
			{
				throw new ConfigurationErrorsException($"Could not find a SecretConfigurationSection called {configurationSectionName}");
			}

			foreach (SecretElement secret in section.Secrets)
			{
				_secrets.Add(secret.Key, secret);
			}
		}

		public string Get(string key)
		{
			SecretElement element;
			if (!_secrets.TryGetValue(key, out element))
			{
				throw new SecretElementNotFoundException(key);
			}

			return _secretStore.Get(element.Application, element.Name).Value;
		}
	}
}
