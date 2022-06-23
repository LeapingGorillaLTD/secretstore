using System;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit.Attributes;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.IntegrationTests.AwsKmsKeyManagerTests.GenerateDataKeyTests
{
	public class WhenNullKeyIdProvided : WhenTestingAwsKmsKeyManager
	{
		private string _keyId;
		private Exception _ex;

		[Given]
		public void WeHaveKeyId()
		{
			_keyId = null;
		}

		[When]
		public void GenerateDataKeyCalled()
		{
			try
			{
				Manager.GenerateDataKey(_keyId);
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
			Assert.That(_ex, Is.TypeOf<ArgumentNullException>());
		}
	}
}
