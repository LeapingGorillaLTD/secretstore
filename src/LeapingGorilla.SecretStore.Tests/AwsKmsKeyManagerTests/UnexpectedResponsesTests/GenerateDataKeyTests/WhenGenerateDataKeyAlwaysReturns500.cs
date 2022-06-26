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
using System.Net;
using Amazon.KeyManagementService.Model;
using LeapingGorilla.SecretStore.Aws.Exceptions;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit.Attributes;
using NSubstitute;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.AwsKmsKeyManagerTests.UnexpectedResponsesTests.GenerateDataKeyTests
{
	public class WhenGenerateDataKeyAlwaysReturns500 : WhenTestingAwsKmsKeyManagerForUnexpectedResponses
	{
		private GenerateDataKeyResponse _result;
		private Exception _ex;

		[Given]
		public void ServiceReturnsFailCode()
		{
			_result = new GenerateDataKeyResponse
			{
				HttpStatusCode = HttpStatusCode.InternalServerError
			};

			KmsService.GenerateDataKeyAsync(Arg.Any<GenerateDataKeyRequest>()).Returns(_result);
		}

		[When]
		public void EncryptIsCalled()
		{
			try
			{
				Manager.GenerateDataKey(KeyId);
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
		public void ExceptionShouldBeExpectedType()
		{
			Assert.That(_ex, Is.TypeOf<KeyManagementServiceUnavailableException>());
		}

		[Then]
		public void ExceptionShouldHaveExpectedCode()
		{
			Assert.That(((KeyManagementServiceUnavailableException)_ex).LastStatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
		}

		[Then]
		public void ShouldRetryExpectedNumberOfTimes()
		{
			KmsService.Received(3).GenerateDataKeyAsync(Arg.Any<GenerateDataKeyRequest>());
		}
	}
}
