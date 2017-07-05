using System.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace LeapingGorilla.SecretStore.Configuration
{
	///<summary>Support class for SecretsFromConfiguration</summary>
	[ExcludeFromCodeCoverage]
	public class SecretElement : ConfigurationElement
	{
		private const string KeyAttr = "key";
		private const string NameAttr = "name";
		private const string ApplicationAttr = "application";

		[ConfigurationProperty(KeyAttr, DefaultValue = "", IsRequired = true, IsKey = true)]
		public string Key
		{
			get => (string)this[KeyAttr];
			set => this[KeyAttr] = value;
		}

		[ConfigurationProperty(NameAttr, DefaultValue = "", IsRequired = true, IsKey = false)]
		public string Name
		{
			get => (string)this[NameAttr];
			set => this[NameAttr] = value;
		}

		[ConfigurationProperty(ApplicationAttr, DefaultValue = "", IsRequired = true, IsKey = false)]
		public string Application
		{
			get => (string)this[ApplicationAttr];
			set => this[ApplicationAttr] = value;
		}

		public SecretElement() { }

		public SecretElement(string name, string application, string key)
		{
			Name = name;
			Application = application;
			Key = key;
		}
	}
}
