﻿// /*
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
using LeapingGorilla.SecretStore.Aws;
using LeapingGorilla.SecretStore.Exceptions;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit.Attributes;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.AwsKmsKeyManagerTests.EncryptDataTests
{
	public class WhenDataIsTooLarge : WhenTestingAwsKmsKeyManagerManager
	{
		private byte[] _data;
		private string _keyId;
		private Exception _ex;

		[Given]
		public void WeHaveData()
		{
			_data = new byte[AwsKmsKeyManager.MaxEncryptPayloadSize + 1];
		}

		[Given]
		public void WeHaveKeyId()
		{
			_keyId = "Test";
		}

		[When]
		public void WeCallEncrypt()
		{
			try
			{
				Manager.EncryptData(_keyId, _data);
			}
			catch (Exception ex)
			{
				_ex = ex;
			}
		}

		[Then]
		public void ExceptionShouldBeThrown()
		{
			Assert.That(_ex, Is.Not.Null);
		}

		[Then]
		public void ExceptionShouldBeExpectedType()
		{
			Assert.That(_ex, Is.TypeOf<PayloadTooLargeException>());
		}
	}
}
