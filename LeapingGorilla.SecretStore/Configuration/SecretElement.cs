using System.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace LeapingGorilla.SecretStore.Configuration
{
	///<summary>Support class for SecretsFromConfiguration</summary>
	[ExcludeFromCodeCoverage]
	public class SecretElement : ConfigurationElement
	{
		private const string NameAttr = "name";
		private const string KeyAttr = "key";

		[ConfigurationProperty(NameAttr, DefaultValue = "", IsRequired = true, IsKey = true)]
		public string Name
		{
			get { return (string)this[NameAttr]; }
			set { this[NameAttr] = value; }
		}

		[ConfigurationProperty(KeyAttr, DefaultValue = "", IsRequired = true, IsKey = false)]
		public string Key
		{
			get { return (string)this[KeyAttr]; }
			set { this[KeyAttr] = value; }
		}

		public SecretElement() { }

		public SecretElement(string name, string key)
		{
			Name = name;
			Key = key;
		}
	}
}
