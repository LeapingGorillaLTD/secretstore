using System;
using System.Security.Cryptography;
using LeapingGorilla.SecretStore.Interfaces;
using LeapingGorilla.SecretStore.Tests.Properties;
using LeapingGorilla.Testing;
using LeapingGorilla.Testing.Attributes;
using NSubstitute;

namespace LeapingGorilla.SecretStore.Tests.RsaKeyManagerTests
{
	public abstract class WhenTestingRsaKeyManager : WhenTestingTheBehaviourOf
	{
		/* 4096-bit key converted to bytes with a SHA-1 header (160-bit) and OAEP padding 
		(https://msdn.microsoft.com/en-us/library/system.security.cryptography.rsacryptoserviceprovider.encrypt(v=vs.110).aspx)*/
		public readonly int ExpectedMaxEncryptionPayloadSizeInBytes = (int)Math.Floor((4096 / 8) - 2 - (2 * (160 / 8m)));

		// Decrypt size == key size
		public readonly int ExpectedMaxDecryptionPayloadSizeInBytes = 4096 / 8; 
		

		[ItemUnderTest]
		public RsaKeyManager Manager { get; set; }

		[Dependency]
		public IRsaKeyStore KeyStore { get; set; }

		[Given]
		public void GetSigningKeyReturnsKeyForSigning()
		{
			RSAParameters privateKey;
			using (var rsa = new RSACryptoServiceProvider())
			{
				rsa.FromXmlString(Resources.TestRsaKey);
				privateKey = rsa.ExportParameters(true);
			}

			KeyStore.GetSigningKey().Returns(privateKey);
		}
	}
}
