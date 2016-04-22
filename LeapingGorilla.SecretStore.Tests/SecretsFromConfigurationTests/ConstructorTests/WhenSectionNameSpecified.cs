using System;
using LeapingGorilla.SecretStore.Configuration;
using LeapingGorilla.Testing.Attributes;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.SecretsFromConfigurationTests.ConstructorTests
{
	public class WhenSectionNameSpecified : WhenTestingSecretsFromConfigConstructor
	{
		private Exception _ex;
		private SecretsFromConfiguration _result;

		[When]
		public void MethodName()
		{
			try
			{
				_result = new SecretsFromConfiguration(SecretStore, "SecretStore");
			}
			catch (Exception e)
			{
				_ex = e;
			}
		}

		[Then]
		public void NoExceptionShouldBeThrown()
		{
			Assert.That(_ex, Is.Null);
		}

		[Then]
		public void ConfigShouldBeLoaded()
		{
			Assert.That(_result, Is.Not.Null);
		}
	}
}
