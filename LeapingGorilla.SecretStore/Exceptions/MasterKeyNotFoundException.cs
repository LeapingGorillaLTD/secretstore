using System;

namespace LeapingGorilla.SecretStore.Exceptions
{
	public class MasterKeyNotFoundException : Exception
	{
		public string KeyId { get; private set; }

		public MasterKeyNotFoundException(string keyId) : base($"Could not find a key with the ID '{keyId}'")
		{
			KeyId = keyId;
		}
	}
}
