using LeapingGorilla.Testing.Attributes;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.IntegrationTests.AwsKmsKeyManagerTests.GenerateDataKeyTests
{
	public class WhenValidKeyIdProvided : WhenTestingAwsKmsKeyManager
	{
		private string _keyId;
		private GenerateDataKeyResult _result;

		[Given]
		public void WeHaveKeyId()
		{
			_keyId = DevelopmentKeyId;
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
