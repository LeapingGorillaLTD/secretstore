using System.Text;
using LeapingGorilla.Testing.Attributes;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.RsaKeyManagerTests.KeyIdSegmentTests
{
	public class WhenDataIsProvided : WhenTestingRsaKeyManager
	{
		private byte[] _protectedData;
		private byte[] _result;
		private string _keyId;

		[Given]
		public void WeHaveKeyId()
		{
			_keyId = "Test Key ID";
		}

		[Given]
		public void WeHaveProtectedData()
		{
			_protectedData = Encoding.UTF8.GetBytes("Sample Data To Protect");
		}

		[When]
		public void WePrependKeyIdSegment()
		{
			_result = Manager.PrependKeyIdSegment(_keyId, _protectedData);
		}

		[Then]
		public void ResultShouldBeReturned()
		{
			Assert.That(_result, Is.Not.Null);
		}

		[Then]
		public void ResultShouldBeExpectedSize()
		{
			Assert.That(_result, Has.Length.EqualTo(553));
		}
	}
}
