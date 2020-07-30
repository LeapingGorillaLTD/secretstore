using System;
using LeapingGorilla.Testing.Attributes;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.SecretStoreTests
{
	public class WhenCallingProtectWithNull : WhenTestingSecretStore
	{
		private Exception _thrownEx;

		[When]
		public void ProtectCalled()
		{
			try
			{
				SecretStore.Protect("Test", null);
			}
			catch (Exception ex)
			{
				_thrownEx = ex;
			}
		}

		[Then]
		public void ExceptionThrown()
		{
			Assert.That(_thrownEx, Is.Not.Null);
		}

		[Then]
		public void ExceptionExpectedType()
		{
			Assert.That(_thrownEx, Is.TypeOf<ArgumentNullException>());
		}
	}
}
