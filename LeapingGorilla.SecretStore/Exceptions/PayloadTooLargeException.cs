using System;

namespace LeapingGorilla.SecretStore.Exceptions
{
	public class PayloadTooLargeException : Exception
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
