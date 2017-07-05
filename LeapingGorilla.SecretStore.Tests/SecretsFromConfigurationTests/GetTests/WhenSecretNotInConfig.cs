using System;
using LeapingGorilla.SecretStore.Exceptions;
using LeapingGorilla.Testing.Attributes;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.SecretsFromConfigurationTests.GetTests
{
	public class WhenSecretNotInConfig : WhenTestingSecretsFromConfiguration
	{
		private string _key;
		private Exception _ex;

		[Given]
		public void WeHaveSecretDetails()
		{
			_key = "Missing";
		}

		[When]
		public void WeGetSecret()
		{
			try
			{
				SecretsConfig.Get(_key);
			}
			catch (Exception e)
			{
				_ex = e;
			}
		}

		[Then]
		public void ExceptionShouldOccur()
		{
			Assert.That(_ex, Is.Not.Null);
		}

		[Then]
		public void ExceptionShouldBeExpectedType()
		{
			Assert.That(_ex, Is.TypeOf<SecretElementNotFoundException>());
		}

		[Then]
		public void ExceptionShouldHaveMissingSecretDetails()
		{
			var e = (SecretElementNotFoundException)_ex;
			Assert.That(e.Key, Is.EqualTo(_key));
		}
	}
}
