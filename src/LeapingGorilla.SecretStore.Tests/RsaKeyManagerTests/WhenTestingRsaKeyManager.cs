// /*
//    Copyright 2013-2022 Leaping Gorilla LTD
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//        http://www.apache.org/licenses/LICENSE-2.0
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// */

using System;
using System.Security.Cryptography;
using System.Xml;
using LeapingGorilla.SecretStore.Interfaces;
using LeapingGorilla.SecretStore.Tests.Properties;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit;
using NSubstitute;

namespace LeapingGorilla.SecretStore.Tests.RsaKeyManagerTests
{
	public abstract class WhenTestingRsaKeyManager : WhenTestingTheBehaviourOf
	{
		/* 4096-bit key converted to bytes with a SHA-1 header (160-bit) and OAEP padding 
		(https://msdn.microsoft.com/en-us/library/system.security.cryptography.rsacryptoserviceprovider.encrypt(v=vs.110).aspx)*/
		public readonly int ExpectedMaxEncryptionPayloadSizeInBytes = (int)Math.Floor((4096 / 8) - 2 - (2 * (160 / 8m)));

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
