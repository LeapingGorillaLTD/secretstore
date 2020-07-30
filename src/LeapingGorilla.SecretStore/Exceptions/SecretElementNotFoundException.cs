namespace LeapingGorilla.SecretStore.Exceptions
{
	///<summary>
	/// Thrown if a named secret cannot be found in the config file. The key is case sensitive
	/// </summary>
	public class SecretElementNotFoundException : SecretStoreException
	{
		public string Key { get; }

		public SecretElementNotFoundException(string key) : base($"No secret element could be found in config with a key of {key}. The key is case sensitive.")
		{
			Key = key;
		}
	}
}
