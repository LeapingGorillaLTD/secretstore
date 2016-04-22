namespace LeapingGorilla.SecretStore.Interfaces
{
	public interface ISecrets
	{
		///<summary>
		/// Retrieves the name of a secret in the Secret Store looked up using the 
		/// name of the secret in our configuration.
		/// </summary>
		string Get(string name);
	}
}
