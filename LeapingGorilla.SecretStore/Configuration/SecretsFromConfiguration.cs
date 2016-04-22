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
	/// It is recommended that you create an instance of this class as a singleton or cache your 
	/// instance for best performance. 
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
	/// 			&lt;add name="MyNameInCode" key="NameOfSecretInSecretStore" /&gt;
	/// 			&lt;add name="MyOtherNameInCode" key="NameOfAnotherSecretInSecretStore" /&gt;
	/// 			&lt;add name="RedisConnectionString" key="RedisConnectionString.Dev" /&gt;
	/// 			&lt;add name="DBConnectionString" key="DBConnectionString.Dev" /&gt;
	/// 		&lt;/Secrets&gt;
	/// 	&lt;/SecretStore&gt;
	/// &lt;/configuration&gt;
	/// 
	/// We have a name and key structure so that you can use the name in code and apply config transforms 
	/// to use different keys on different environments without a code change.
	/// </remarks>
	public class SecretsFromConfiguration : ISecrets
	{
		private readonly Dictionary<string, string> _secrets = new Dictionary<string, string>();
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
		/// <param name="configurationSectionName">Name of the configuration section.</param>
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
				_secrets.Add(secret.Name, secret.Key);
			}
		}

		public string Get(string name)
		{
			string key;
			if (!_secrets.TryGetValue(name, out key))
			{
				throw new SecretNotFoundException(name);
			}

			return _secretStore.Get(key).Value;
		}
	}
}
