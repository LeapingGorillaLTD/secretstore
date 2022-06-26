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

using System.Text;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit.Attributes;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.EncryptionManagerTests
{
	public class WhenRoundTrippingSimpleString : WhenTestingEncryptionManager
	{
		private string _plainText;
		private EncryptionResult _result;
		private byte[] _decryptResult;

		[Given]
		public void WeHaveDataToEncrypt()
		{
			_plainText = "Sample Setting Value";
		}

		[When]
		public void WeEncryptAndDecryptTheString()
		{
			_result = Manager.Encrypt(ValidKeyId, Encoding.UTF8.GetBytes(_plainText));
			_decryptResult = Manager.Decrypt(ValidKeyId, _result.EncryptedDataKey, _result.InitialisationVector, _result.EncryptedData);
		}

		[Then]
		public void WeShouldReceiveResult()
		{
			Assert.That(_result, Is.Not.Null);
		}

		[Then]
		public void WeShouldHaveDecryptedResult()
		{
			Assert.That(_decryptResult, Is.Not.Null);
		}

		[Then]
		public void DecryptedResultShouldBeStartingString()
		{
			var str = Encoding.UTF8.GetString(_decryptResult);
			Assert.That(str, Is.EqualTo(_plainText));
		}
	}
}
