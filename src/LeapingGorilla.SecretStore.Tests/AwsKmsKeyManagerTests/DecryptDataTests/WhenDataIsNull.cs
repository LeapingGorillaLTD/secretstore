using System;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit.Attributes;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.AwsKmsKeyManagerTests.DecryptDataTests
{
	public class WhenDataIsNull : WhenTestingAwsKmsKeyManagerManager
	{
		private byte[] _data;
		private Exception _ex;

		[Given]
		public void WeHaveData()
		{
			_data = null;
		}
		
		[When]
		public void WeCallEncrypt()
		{
			try
			{
				Manager.DecryptData(_data);
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