using System;
using System.Configuration;
using LeapingGorilla.SecretStore.Configuration;
using LeapingGorilla.Testing.Attributes;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.SecretsFromConfigurationTests.ConstructorTests
{
	public class WhenSectionNameIncorrect : WhenTestingSecretsFromConfigConstructor
	{
		private Exception _ex;
		private SecretsFromConfiguration _result;

		[When]
		public void MethodName()
		{
			try
			{
				_result = new SecretsFromConfiguration(SecretStore, "WrongSecretStore");
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
		public void ConfigShouldNotBeLoaded()
		{
			Assert.That(_result, Is.Null);
		}

		[Then]
		public void ExceptionShouldBeExpectedType()
		{
			Assert.That(_ex, Is.TypeOf<ConfigurationErrorsException>());
		}
	}
}
