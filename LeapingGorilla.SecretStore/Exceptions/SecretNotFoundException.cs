namespace LeapingGorilla.SecretStore.Exceptions
{
	///<summary>
	/// Thrown if a named secret cannot be found in the backing data store. Secret names may
	/// be case sensitive dependant on the backing data store.
	/// </summary>
	public class SecretNotFoundException : SecretStoreException
	{
		public string SecretName { get; private set; }

		public SecretNotFoundException(string secretName) : base($"No secret could be found with the name {secretName}. Your secret name may be case sensitive depending on your backing data store")
		{
			SecretName = secretName;
		}
	}
}
