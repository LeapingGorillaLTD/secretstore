using System;
using System.Linq;
using System.Security.Cryptography;
using LeapingGorilla.SecretStore.Exceptions;
using LeapingGorilla.SecretStore.Tests.Properties;
using LeapingGorilla.Testing.Attributes;
using NSubstitute;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.RsaKeyManagerTests.DecryptDataTests
{
	public class WhenDataIsTamperedWith : WhenTestingRsaKeyManager
	{
		private byte[] _data;
		private string _keyId;
		private Exception _ex;

		[Given(0)]
		public void WeHaveKeyId()
		{
			_keyId = "Test";
		}

		[Given(1)]
		public void WeHaveData()
		{
			var temp = Manager.EncryptData(_keyId, new byte[ExpectedMaxEncryptionPayloadSizeInBytes]).ToList();
			temp.Add(1);
			_data = temp.ToArray();
			temp.Clear();
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
		public void WeCallDecrypt()
		{
			try
			{
				Manager.DecryptData(_data);
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
			Assert.That(_ex, Is.TypeOf<InvalidDataFormatException>());
		}
	}
}
