﻿using System;
using LeapingGorilla.SecretStore.Aws;
using LeapingGorilla.SecretStore.Exceptions;
using LeapingGorilla.Testing.Attributes;
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
