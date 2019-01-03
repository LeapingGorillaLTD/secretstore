using System.Security.Cryptography;
using LeapingGorilla.SecretStore.Tests.Properties;
using LeapingGorilla.Testing.Attributes;
using NSubstitute;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.RsaKeyManagerTests.GenerateDataKeyTests
{
	public class WhenValidKeyIdProvided : WhenTestingRsaKeyManager
	{
		private string _keyId;
		private GenerateDataKeyResult _result;

		[Given]
		public void WeHaveKeyId()
		{
			_keyId = "Test";
		}

		[Given]
		public void KeyStoreRecognisesIdForPublicKey()
		{
			RSAParameters rsaParams;
			using (var rsa = new RSACryptoServiceProvider())
			{
				rsa.ImportParameters(PrivateTestKey);
				rsaParams = rsa.ExportParameters(false);
			}

			KeyStore.GetPublicKey(Arg.Any<string>()).Returns(rsaParams);
		}

		[When]
		public void GenerateDataKeyCalled()
		{
			_result = Manager.GenerateDataKey(_keyId);
		}

		[Then]
		public void ResultShouldBeReturned()
		{
			Assert.That(_result, Is.Not.Null);
		}

		[Then]
		public void ResultShouldContainExpectedPlainText()
		{
			Assert.That(_result.PlainTextKey, Is.Not.Null);
			Assert.That(_result.PlainTextKey, Has.Length.EqualTo(RsaKeyManager.DataKeyLengthInBytes));
		}

		[Then]
		public void ResultShouldContainExpectedCipherText()
		{
			Assert.That(_result.CipherTextKey, Is.Not.Null);
		}

		[Then]
		public void ResultShouldContainExpectedKeyId()
		{
			Assert.That(_result.KeyId, Is.EqualTo(_keyId));
		}
	}
}
