namespace LeapingGorilla.SecretStore
{
	///<summary>An unprotected secret</summary>
	public class ClearSecret : Secret
	{
		///<summary></summary>
		public string Value { get; set; }
		

		public ClearSecret(string applicationName, string secretName, string secretValue)
			:base(applicationName, secretName)
		{
			Value = secretValue;
		}
	}
}
