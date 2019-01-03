using System;
using System.Security.Cryptography;
using System.Xml;
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

		protected RSAParameters PrivateTestKey;


		[ItemUnderTest]
		public RsaKeyManager Manager { get; set; }

		[Dependency]
		public IRsaKeyStore KeyStore { get; set; }

		[Given(-1)]
		public void GetSigningKeyReturnsKeyForSigning()
		{
			PrivateTestKey = new RSAParameters();
			XmlDocument xml = new XmlDocument();
			xml.LoadXml(Resources.TestRsaKey);

			foreach (XmlNode node in xml.DocumentElement.ChildNodes)
			{
				switch (node.Name)
				{
					case "Modulus":     PrivateTestKey.Modulus =    Convert.FromBase64String(node.InnerText); break;
					case "Exponent":    PrivateTestKey.Exponent =   Convert.FromBase64String(node.InnerText); break;
					case "P":           PrivateTestKey.P =          Convert.FromBase64String(node.InnerText); break;
					case "Q":           PrivateTestKey.Q =          Convert.FromBase64String(node.InnerText); break;
					case "DP":          PrivateTestKey.DP =         Convert.FromBase64String(node.InnerText); break;
					case "DQ":          PrivateTestKey.DQ =         Convert.FromBase64String(node.InnerText); break;
					case "InverseQ":    PrivateTestKey.InverseQ =   Convert.FromBase64String(node.InnerText); break;
					case "D":           PrivateTestKey.D =          Convert.FromBase64String(node.InnerText); break;
				}
			}
			
			KeyStore.GetSigningKey().Returns(PrivateTestKey);
		}
	}
}
