using System;
using LeapingGorilla.Testing.Attributes;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.RsaKeyManagerTests.DecryptDataTests
{
	public class WhenDataIsEmpty : WhenTestingRsaKeyManager
	{
		private byte[] _data;
		private string _keyId;
		private Exception _ex;

		[Given]
		public void WeHaveData()
		{
			_data = new byte[0];
		}

		[Given]
		public void WeHaveKeyId()
		{
			_keyId = "Test";
		}

		[When]
		public void WeCallDecrypt()
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
			Assert.That(_ex, Is.TypeOf<ArgumentException>());
		}
	}
}
