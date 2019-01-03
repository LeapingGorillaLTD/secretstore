using System;
using System.Net;
using Amazon.KeyManagementService.Model;
using LeapingGorilla.SecretStore.Aws.Exceptions;
using LeapingGorilla.Testing.Attributes;
using NSubstitute;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.AwsKmsKeyManagerTests.UnexpectedResponsesTests.DecryptTests
{
	public class WhenDecryptAlwaysReturns503 : WhenTestingAwsKmsKeyManagerForUnexpectedResponses
	{
		private DecryptResponse _result;
		private byte[] _data;
		private Exception _ex;

		[Given]
		public void WeHaveData()
		{
			_data = new byte[15];
		}
		
		[Given]
		public void ServiceReturnsFailCode()
		{
			_result = new DecryptResponse
			{
				HttpStatusCode = HttpStatusCode.ServiceUnavailable
			};

			KmsService.DecryptAsync(Arg.Any<DecryptRequest>())
				.Returns(_result);
		}

		[When]
		public void EncryptIsCalled()
		{
			try
			{
				Manager.DecryptData(_data);
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
			KmsService.Received(3).DecryptAsync(Arg.Any<DecryptRequest>());
		}
	}
}
