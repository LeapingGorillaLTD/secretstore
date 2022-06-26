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

using System;
using LeapingGorilla.SecretStore.Exceptions;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit.Attributes;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.RsaKeyManagerTests.KeyIdSegmentTests
{
	public class WhenDataTooShort : WhenTestingRsaKeyManager
	{
		private byte[] _protectedData;
		private string _result;
		private int _segmentLength;
		private Exception _ex;

		[Given]
		public void WeHaveProtectedData()
		{
			_protectedData = new byte[4];
		}

		[When]
		public void GetKeyIdCalled()
		{
			try
			{
				_result = Manager.GetKeyId(_protectedData, out _segmentLength);
			}
			catch (Exception e)
			{
				_ex = e;
			}
		}

		[Then]
		public void ResultShouldNotBeReturned()
		{
			Assert.That(_result, Is.Null);
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
