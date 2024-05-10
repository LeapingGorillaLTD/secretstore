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

using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit.Attributes;

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