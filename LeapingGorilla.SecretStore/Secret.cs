namespace LeapingGorilla.SecretStore
{
	///<summary>An unprotected secret</summary>
	public class Secret
	{
		public string ApplicationName { get; set; }
		public string SecretName { get; set; }
		public string Value { get; set; }

		public Secret() { }

		public Secret(string applicationName, string secretName, string secretValue)
		{
			ApplicationName = applicationName;
			SecretName = secretName;
			Value = secretValue;
		}
	}
}
