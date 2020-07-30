using LeapingGorilla.Testing.Attributes;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.IntegrationTests.AwsKmsKeyManagerTests.GenerateDataKeyTests
{
	public class WhenDataIsRoundTripped : WhenTestingAwsKmsKeyManager
	{
		private string _keyId;
		private GenerateDataKeyResult _result;
		private byte[] _decryptedCipherText;

		[Given]
		public void WeHaveKeyId()
		{
			_keyId = DevelopmentKeyId;
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
