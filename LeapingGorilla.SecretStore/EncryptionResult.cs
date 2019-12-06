namespace LeapingGorilla.SecretStore
{
	public class EncryptionResult
	{
		///<summary>Encrypted data key which was used to protect the data</summary>
		public byte[] EncryptedDataKey { get; set; }

		///<summary>The IV used for encryption</summary>
		public byte[] InitialisationVector { get; set; }

		///<summary>The encrypted data</summary>
		public byte[] EncryptedData { get; set; }

		public EncryptionResult() { }

		public EncryptionResult(byte[] iv, byte[] encryptedDataKey, byte[] encryptedData)
		{
			InitialisationVector = iv;
			EncryptedDataKey = encryptedDataKey;
			EncryptedData = encryptedData;
		}
	}
}
