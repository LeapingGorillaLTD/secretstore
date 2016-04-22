namespace LeapingGorilla.SecretStore.Exceptions
{
	///<summary>Thrown if a named secret cannot be found in the backing datastore</summary>
	public class SecretNotFoundException : SecretStoreException
	{
		public string SecretName { get; private set; }

		public SecretNotFoundException(string secretName) : base($"No secret could be found with the name {secretName}")
		{
			SecretName = secretName;
		}
	}
}
