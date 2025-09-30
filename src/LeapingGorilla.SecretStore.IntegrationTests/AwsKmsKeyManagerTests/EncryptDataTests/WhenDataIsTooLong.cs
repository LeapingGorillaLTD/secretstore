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

using System.Linq;
using LeapingGorilla.SecretStore.Aws;
using LeapingGorilla.SecretStore.Exceptions;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit.Attributes;

namespace LeapingGorilla.SecretStore.IntegrationTests.AwsKmsKeyManagerTests.EncryptDataTests
{
	public class WhenDataIsTooLong : WhenTestingAwsKmsKeyManager
	{
		private byte[] _data;
		private string _keyId;
		private byte[] _result;
		
		[Given]
		public void WeHaveData()
		{
			_data = Enumerable.Repeat<byte>(1, AwsKmsKeyManager.MaxEncryptPayloadSize + 1).ToArray();
		}
		
		[Given]
		public void WeHaveKeyId()
		{
			_keyId = DevelopmentKeyId;
		}

		[When(DoNotRethrowExceptions: true)]
		public void WeCallEncrypt()
		{
			_result = Manager.EncryptData(_keyId, _data);
		}

		[Then]
		public void ExceptionShouldBeThrown()
		{
			Assert.That(ThrownException, Is.Not.Null);
		}

		[Then]
		public void ExceptionShouldBeExpectedType()
		{
			Assert.That(ThrownException, Is.TypeOf<PayloadTooLargeException>());
		}
	}
}