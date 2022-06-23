using System.Linq;
using System.Security.Cryptography;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit.Attributes;
using NSubstitute;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.RsaKeyManagerTests.EncryptDataTests
{
	public class WhenDataIsMaxLength : WhenTestingRsaKeyManager
	{
		private byte[] _data;
		private string _keyId;
		private byte[] _result;
		
		[Given]
		public void WeHaveData()
		{
			_data = Enumerable.Repeat<byte>(1, ExpectedMaxEncryptionPayloadSizeInBytes).ToArray();
		}

		[Given]
		public void KeyStoreRecognisesIdForPublicKey()
		{
			RSAParameters publicKey;
			RSAParameters privateKey;
			using (var rsa = new RSACryptoServiceProvider())
			{
				rsa.ImportParameters(PrivateTestKey);
				publicKey = rsa.ExportParameters(false);
				privateKey = rsa.ExportParameters(true);
			}

			KeyStore.GetPublicKey(Arg.Any<string>()).Returns(publicKey);
			KeyStore.GetPrivateKey(Arg.Any<string>()).Returns(privateKey);
		}

		[Given]
		public void WeHaveKeyId()
		{
			_keyId = "Test";
		}

		[When]
		public void WeCallEncrypt()
		{
			_result = Manager.EncryptData(_keyId, _data);
		}

		[Then]
		public void ResultShouldBeReturned()
		{
			Assert.That(_result, Is.Not.Null);
		}

		[Then]
		public void ResultShouldContainCipherText()
		{
			Assert.That(_result, Is.Not.Null);
			Assert.That(_result, Has.Length.GreaterThan(1));
		}
	}
}
