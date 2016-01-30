using System;
using LeapingGorilla.SecretStore.Interfaces;
using LeapingGorilla.Testing;
using LeapingGorilla.Testing.Attributes;

namespace LeapingGorilla.SecretStore.Tests.RsaKeyManagerTests
{
	public abstract class WhenTestingRsaKeyManager : WhenTestingTheBehaviourOf
	{
		/* 4096-bit key converted to bytes with a SHA-1 header (160-bit) and OAEP padding 
		(https://msdn.microsoft.com/en-us/library/system.security.cryptography.rsacryptoserviceprovider.encrypt(v=vs.110).aspx)*/
		public readonly int MaxEncryptionPayloadSizeInBytes = (int)Math.Floor((4096 / 8) - 2 - (2 * (160 / 8m)));

		// Decrypt size == key size
		public readonly int MaxDecryptionPayloadSizeInBytes = 4096 / 8; 
		

		[ItemUnderTest]
		public RsaKeyManager Manager { get; set; }

		[Dependency]
		public IRsaKeyStore KeyStore { get; set; }
	}
}
