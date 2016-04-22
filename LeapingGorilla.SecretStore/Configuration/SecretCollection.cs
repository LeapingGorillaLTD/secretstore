using System.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace LeapingGorilla.SecretStore.Configuration
{
	///<summary>Support class for SecretsFromConfiguration</summary>
	[ExcludeFromCodeCoverage]
	public class SecretCollection : ConfigurationElementCollection
	{
		public void Add(SecretElement secretElement)
		{
			BaseAdd(secretElement);
		}

		public void Clear()
		{
			BaseClear();
		}

		protected override ConfigurationElement CreateNewElement()
		{
			return new SecretElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((SecretElement)element).Name;
		}

		public void Remove(SecretElement secretElement)
		{
			BaseRemove(secretElement.Name);
		}

		public void RemoveAt(int index)
		{
			BaseRemoveAt(index);
		}

		public void Remove(string name)
		{
			BaseRemove(name);
		}
	}
}
