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

namespace LeapingGorilla.SecretStore.Tests.RsaKeyManagerTests.KeyIdSegmentTests
{
	public class WhenDataIsRoundTripped : WhenTestingRsaKeyManager
	{
		private byte[] _protectedData;
		private byte[] _result;
		private string _returnedKeyId;
		private int _returnedKeySegmentSize;
		private string _keyId;

		[Given]
		public void WeHaveKeyId()
		{
			_keyId = "Test Key ID";
		}

		[Given]
		public void WeHaveProtectedData()
		{
			_protectedData = Encoding.UTF8.GetBytes("Sample Data To Protect");
		}

		[When]
		public void WeRoundTripTheData()
		{
			_result = Manager.PrependKeyIdSegment(_keyId, _protectedData);
			_returnedKeyId = Manager.GetKeyId(_result, out _returnedKeySegmentSize);
		}

		[Then]
		public void ResultShouldBeReturned()
		{
			Assert.That(_result, Is.Not.Null);
		}

		[Then]
		public void ResultShouldHaveExpectedSize()
		{
			Assert.That(_result, Has.Length.EqualTo(553));
		}

		[Then]
		public void KeyIdSegmentShouldBeExpectedSize()
		{
			Assert.That(_returnedKeySegmentSize, Is.EqualTo(531));
		}

		[Then]
		public void ReturnedKeyIdShouldBeAsExpected()
		{
			Assert.That(_returnedKeyId, Is.EqualTo(_keyId));
		}
	}
}
