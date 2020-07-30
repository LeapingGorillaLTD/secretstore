namespace LeapingGorilla.SecretStore.Interfaces
{
	public interface ISecrets
	{
		///<summary>
		/// Retrieves the value of a secret from the secret store. The secret will be looked up using the
		/// </summary>
		/// <param name="key">Key of the secret to retrieve</param>
		/// <returns>Value of the secret as retrieved from the Secret Store</returns>
		string Get(string key);
	}
}
