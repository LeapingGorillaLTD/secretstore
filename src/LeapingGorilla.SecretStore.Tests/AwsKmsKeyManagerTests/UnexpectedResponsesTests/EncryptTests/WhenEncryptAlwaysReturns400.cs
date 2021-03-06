﻿using System;
using System.Net;
using Amazon.KeyManagementService.Model;
using LeapingGorilla.SecretStore.Aws.Exceptions;
using LeapingGorilla.Testing.Attributes;
using NSubstitute;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.AwsKmsKeyManagerTests.UnexpectedResponsesTests.EncryptTests
{
	public class WhenEncryptAlwaysReturns400 : WhenTestingAwsKmsKeyManagerForUnexpectedResponses
	{
		private EncryptResponse _result;
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
			_result = new EncryptResponse
			{
				CiphertextBlob = null,
				HttpStatusCode = HttpStatusCode.BadRequest
			};

			KmsService.EncryptAsync(Arg.Any<EncryptRequest>())
				.Returns(_result);
		}

		[When]
		public void EncryptIsCalled()
		{
			try
			{
				Manager.EncryptData(KeyId, _data);
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
			Assert.That(((KeyManagementServiceUnavailableException)_ex).LastStatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
		}

		[Then]
		public void ShouldNotRetry()
		{
			KmsService.Received(1).EncryptAsync(Arg.Any<EncryptRequest>());
		}
	}
}
