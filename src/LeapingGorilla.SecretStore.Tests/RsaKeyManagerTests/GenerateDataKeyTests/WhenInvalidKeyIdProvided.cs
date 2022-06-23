using System;
using LeapingGorilla.SecretStore.Exceptions;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit.Attributes;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.RsaKeyManagerTests.GenerateDataKeyTests
{
	public class WhenInvalidKeyIdProvided : WhenTestingRsaKeyManager
	{
		private string _keyId;
		private Exception _ex;

		[Given]
		public void WeHaveKeyId()
		{
			_keyId = "Fail";
			KeyStore.GetPublicKey(_keyId).Throws(new MasterKeyNotFoundException(_keyId));
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
			Assert.That(_ex, Is.TypeOf<MasterKeyNotFoundException>());
		}

		[Then]
		public void ExceptionShouldHaveKeyName()
		{
			var ex = (MasterKeyNotFoundException)_ex;
			Assert.That(ex.KeyId, Is.EqualTo(_keyId));
		}
	}
}
