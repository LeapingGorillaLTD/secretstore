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
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit.Attributes;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.SecretStoreTests
{
	public class WhenCallingProtectWithNull : WhenTestingSecretStore
	{
		private Exception _thrownEx;

		[When]
		public void ProtectCalled()
		{
			try
			{
				SecretStore.Protect("Test", null);
			}
			catch (Exception ex)
			{
				_thrownEx = ex;
			}
		}

		[Then]
		public void ExceptionThrown()
		{
			Assert.That(_thrownEx, Is.Not.Null);
		}

		[Then]
		public void ExceptionExpectedType()
		{
			Assert.That(_thrownEx, Is.TypeOf<ArgumentNullException>());
		}
	}
}
