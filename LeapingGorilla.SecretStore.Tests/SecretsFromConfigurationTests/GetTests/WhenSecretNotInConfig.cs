using System;
using LeapingGorilla.SecretStore.Exceptions;
using LeapingGorilla.Testing.Attributes;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.SecretsFromConfigurationTests.GetTests
{
	public class WhenSecretNotInConfig : WhenTestingSecretsFromConfiguration
	{
		private string _name;
		private Exception _ex;

		[Given]
		public void WeHaveSecretDetails()
		{
			_name = "Missing";
		}

		[When]
		public void WeGetSecret()
		{
			try
			{
				SecretsConfig.Get(_name);
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
			Assert.That(_ex, Is.TypeOf<SecretNotFoundException>());
		}

		[Then]
		public void ExceptionShouldHaveMissingSecretDetails()
		{
			var e = (SecretNotFoundException)_ex;
			Assert.That(e.SecretName, Is.EqualTo(_name));
		}
	}
}
