namespace LeapingGorilla.SecretStore
{
	public class GenerateDataKeyResult
	{
		///<summary>Plain Text key which you can use for encryption or decryption. This should be discarded after use</summary>
		public byte[] PlainTextKey { get; set; }

		///<summary>Plain text key which has been encrypted by a Master Key. This should be stored alongside the item you are encrypting</summary>
		public byte[] CipherTextKey { get; set; }

		///<summary>ID of the master key used to generate and protect this data key</summary>
		public string KeyId { get; set; }
	}
}
