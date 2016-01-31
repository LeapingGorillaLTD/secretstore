using System;
using System.Net;
using Amazon.KeyManagementService.Model;
using LeapingGorilla.SecretStore.Aws.Exceptions;
using LeapingGorilla.Testing.Attributes;
using NSubstitute;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.AwsKmsKeyManagerTests.UnexpectedResponsesTests.GenerateDataKeyTests
{
	public class WhenGenerateDataKeyAlwaysReturns503 : WhenTestingAwsKmsKeyManagerForUnexpectedResponses
	{
		private GenerateDataKeyResponse _result;
		private Exception _ex;

		[Given]
		public void ServiceReturnsFailCode()
		{
			_result = new GenerateDataKeyResponse
			{
				HttpStatusCode = HttpStatusCode.ServiceUnavailable
			};

			KmsService.GenerateDataKey(Arg.Any<GenerateDataKeyRequest>()).Returns(_result);
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
			Assert.That(((KeyManagementServiceUnavailableException)_ex).LastStatusCode, Is.EqualTo(HttpStatusCode.ServiceUnavailable));
		}

		[Then]
		public void ShouldRetryExpectedNumberOfTimes()
		{
			KmsService.Received(3).GenerateDataKey(Arg.Any<GenerateDataKeyRequest>());
		}
	}
}
