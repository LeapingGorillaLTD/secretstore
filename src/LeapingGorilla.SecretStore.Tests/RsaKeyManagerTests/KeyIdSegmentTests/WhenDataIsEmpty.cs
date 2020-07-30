using System;
using LeapingGorilla.SecretStore.Exceptions;
using LeapingGorilla.Testing.Attributes;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.RsaKeyManagerTests.KeyIdSegmentTests
{
	public class WhenDataIsEmpty : WhenTestingRsaKeyManager
	{
		private byte[] _protectedData;
		private string _result;
		private int _segmentLength;
		private Exception _ex;

		[Given]
		public void WeHaveProtectedData()
		{
			_protectedData = new byte[0];
		}

		[When]
		public void GetKeyIdCalled()
		{
			try
			{
				_result = Manager.GetKeyId(_protectedData, out _segmentLength);
			}
			catch (Exception e)
			{
				_ex = e;
			}
		}

		[Then]
		public void ResultShouldNotBeReturned()
		{
			Assert.That(_result, Is.Null);
		}

		[Then]
		public void ExceptionShouldBeThrown()
		{
			Assert.That(_ex, Is.Not.Null);
		}

		[Then]
		public void ExceptionShouldBeExpectedType()
		{
			Assert.That(_ex, Is.TypeOf<InvalidDataFormatException>());
		}
	}
}
