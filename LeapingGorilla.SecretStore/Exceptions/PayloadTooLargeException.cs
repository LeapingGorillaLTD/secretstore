namespace LeapingGorilla.SecretStore.Exceptions
{
	///<summary>Thrown if the payload which is provided for encryption is too large to be encrytpted</summary>
	/// <remarks>This is mainly relevant to RSA encryption where the max encryptable payload is a function of the key size</remarks>
	public class PayloadTooLargeException : SecretStoreException
	{
		public int ProvidedPayloadSize { get; private set; }
		public int MaxPayloadSize { get; private set; }

		public PayloadTooLargeException(int maxSize, int providedSize) : base($"The payload you provided was larger than the maximum permitted. You passed: {providedSize} bytes, Max: {maxSize} bytes")
		{
			ProvidedPayloadSize = providedSize;
			MaxPayloadSize = maxSize;
		}
	}
}
