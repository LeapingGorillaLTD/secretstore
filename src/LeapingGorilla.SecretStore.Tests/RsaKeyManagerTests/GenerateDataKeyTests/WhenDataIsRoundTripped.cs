using System.Security.Cryptography;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit.Attributes;
using NSubstitute;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.RsaKeyManagerTests.GenerateDataKeyTests
{
	public class WhenDataIsRoundTripped : WhenTestingRsaKeyManager
	{
		private string _keyId;
		private GenerateDataKeyResult _result;
		private byte[] _decryptedCipherText;

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
		public void DataKeyIsGeneratedAndRoundTripped()
		{
			_result = Manager.GenerateDataKey(_keyId);
			_decryptedCipherText = Manager.DecryptData(_result.CipherTextKey);
		}

		[Then]
		public void ResultShouldBeReturned()
		{
			Assert.That(_result, Is.Not.Null);
		}

		[Then]
		public void DecryptedDataShouldBeReturned()
		{
			Assert.That(_decryptedCipherText, Is.Not.Null);
		}

		[Then]
		public void ResultShouldContainPlainText()
		{
			Assert.That(_result.PlainTextKey, Is.Not.Null);
			Assert.That(_result.PlainTextKey, Has.Length.EqualTo(RsaKeyManager.DataKeyLengthInBytes));
		}

		[Then]
		public void ResultShouldContainCipherText()
		{
			Assert.That(_result.CipherTextKey, Is.Not.Null);
		}

		[Then]
		public void DecryptedValueShouldMatchPlainText()
		{
			Assert.That(_decryptedCipherText, Is.EqualTo(_result.PlainTextKey));
		}

		[Then]
		public void ResultShouldContainExpectedKeyId()
		{
			Assert.That(_result.KeyId, Is.EqualTo(_keyId));
		}
	}
}
