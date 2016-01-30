namespace LeapingGorilla.SecretStore
{
	public class GenerateDataKeyResult
	{
		public byte[] PlainTextKey { get; set; }
		public byte[] CipherTextKey { get; set; }
		public string KeyId { get; set; }
	}
}
