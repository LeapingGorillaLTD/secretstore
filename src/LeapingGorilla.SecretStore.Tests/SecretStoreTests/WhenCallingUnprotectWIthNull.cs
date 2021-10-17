using System;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit.Attributes;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.SecretStoreTests
{
	public class WhenCallingUnprotectWithNull : WhenTestingSecretStore
	{
		private Exception _thrownEx;

		[When]
		public void UnprotectCalled()
		{
			try
			{
				SecretStore.Unprotect(null);
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
