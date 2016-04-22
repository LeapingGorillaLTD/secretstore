using System.Configuration;

namespace LeapingGorilla.SecretStore.Configuration
{
	public class SecretConfigurationSection : ConfigurationSection
	{
		private const string SecretsAttr = "Secrets";


		[ConfigurationProperty(SecretsAttr, IsDefaultCollection = false)]
		[ConfigurationCollection(typeof(SecretCollection), 
								AddItemName = "add", 
								ClearItemsName = "clear", 
								RemoveItemName = "remove")]
		public SecretCollection Secrets
		{
			get { return (SecretCollection)base[SecretsAttr]; }
		}
	}
}
