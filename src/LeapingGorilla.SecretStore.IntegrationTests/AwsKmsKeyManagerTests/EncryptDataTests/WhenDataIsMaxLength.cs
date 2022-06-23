using System.Linq;
using LeapingGorilla.SecretStore.Aws;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit.Attributes;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.IntegrationTests.AwsKmsKeyManagerTests.EncryptDataTests
{
	public class WhenDataIsMaxLength : WhenTestingAwsKmsKeyManager
	{
		private byte[] _data;
		private string _keyId;
		private byte[] _result;
		
		[Given]
		public void WeHaveData()
		{
			_data = Enumerable.Repeat<byte>(1, AwsKmsKeyManager.MaxEncryptPayloadSize).ToArray();
		}
		
		[Given]
		public void WeHaveKeyId()
		{
			_keyId = DevelopmentKeyId;
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
