using System;
using System.Security.Cryptography;
using LeapingGorilla.SecretStore.Exceptions;
using LeapingGorilla.SecretStore.Tests.Properties;
using LeapingGorilla.Testing.Attributes;
using NSubstitute;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.RsaKeyManagerTests.EncryptDataTests
{
	public class WhenDataIsTooLarge : WhenTestingRsaKeyManager
	{
		private byte[] _data;
		private string _keyId;
		private Exception _ex;

		[Given]
		public void WeHaveData()
		{
			_data = new byte[ExpectedMaxEncryptionPayloadSizeInBytes + 1];
		}

		[Given]
		public void WeHaveKeyId()
		{
			_keyId = "Test";
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

		[When]
		public void WeCallEncrypt()
		{
			try
			{
				Manager.EncryptData(_keyId, _data);
			}
			catch (Exception ex)
			{
				_ex = ex;
			}
		}

		[Then]
		public void ExceptionShouldBeThrown()
		{
			Assert.That(_ex, Is.Not.Null);
		}

		[Then]
		public void ExceptionShouldBeExpectedType()
		{
			Assert.That(_ex, Is.TypeOf<PayloadTooLargeException>());
		}

		[Then]
		public void ExceptionShouldContainExpectedPayloadSizes()
		{
			var ex = (PayloadTooLargeException)_ex;
			Assert.That(ex.MaxPayloadSize, Is.EqualTo(ExpectedMaxEncryptionPayloadSizeInBytes));
			Assert.That(ex.ProvidedPayloadSize, Is.EqualTo(_data.Length));
		}
	}
}
