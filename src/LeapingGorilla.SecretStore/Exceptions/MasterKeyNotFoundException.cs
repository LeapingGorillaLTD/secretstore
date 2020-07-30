namespace LeapingGorilla.SecretStore.Exceptions
{
	///<summary>Thrown if a master key cannot be found in the key store</summary>
	public class MasterKeyNotFoundException : SecretStoreException
	{
		public string KeyId { get; private set; }

		public MasterKeyNotFoundException(string keyId) : base($"Could not find a key with the ID '{keyId}'")
		{
			KeyId = keyId;
		}
	}
}
