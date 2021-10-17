using System;
using LeapingGorilla.SecretStore.Aws;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit;
using LeapingGorilla.Testing.NUnit.Attributes;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.IntegrationTests.AwsKmsKeyManagerTests
{
	public class WhenNullSystemNameProvided : WhenTestingTheBehaviourOf
	{
		private Exception _ex;
		private AwsKmsKeyManager _manager;
	

		[When]
		public void ManagerInstantiatedWithNullString()
		{
			try
			{
				_manager = new AwsKmsKeyManager(String.Empty);
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
		public void ExceptionShouldBeExpectedTypeThrown()
		{
			Assert.That(_ex, Is.TypeOf<ArgumentException>());
		}



	}
}
