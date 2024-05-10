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
using LeapingGorilla.SecretStore.Aws;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit;
using LeapingGorilla.Testing.NUnit.Attributes;

namespace LeapingGorilla.SecretStore.IntegrationTests.AwsKmsKeyManagerTests
{
	public class WhenNullSystemNameProvided : WhenTestingTheBehaviourOf
	{
		private Exception _ex;
		private AwsKmsKeyManager _manager;
	

		[When]
		public void ManagerInstantiatedWithNullString()
		{
			try
			{
				_manager = new AwsKmsKeyManager(String.Empty);
			}
			catch (Exception e)
			{
				_ex = e;
			}
		}

		[Then]
		public void ExceptionShouldBeThrown()
		{
			Assert.That(_ex, Is.Not.Null);
		}

		[Then]
		public void ExceptionShouldBeExpectedTypeThrown()
		{
			Assert.That(_ex, Is.TypeOf<ArgumentException>());
		}



	}
}
