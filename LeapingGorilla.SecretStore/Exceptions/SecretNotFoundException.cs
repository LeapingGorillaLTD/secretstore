namespace LeapingGorilla.SecretStore.Exceptions
{
	///<summary>
	/// Thrown if a named secret cannot be found in the backing data store. Secret names may
	/// be case sensitive dependant on the backing data store.
	/// </summary>
	public class SecretNotFoundException : SecretStoreException
	{
		public string ApplicationName { get; }
		public string SecretName { get; }

		public SecretNotFoundException(string applicationName, string secretName) : base($"No secret could be found with the name {secretName} for application {applicationName}. Your secret name may be case sensitive depending on your backing data store")
		{
			ApplicationName = applicationName;
			SecretName = secretName;
		}
	}
}
