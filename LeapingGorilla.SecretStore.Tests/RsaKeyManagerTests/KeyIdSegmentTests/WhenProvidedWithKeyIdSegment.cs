using System;
using LeapingGorilla.Testing.Attributes;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.RsaKeyManagerTests.KeyIdSegmentTests
{
	public class WhenProvidedWithKeyIdSegment : WhenTestingRsaKeyManager
	{
		private byte[] _protectedDataWithSegment;
		private string _result;
		private int _keyIdSegmentSize;
		private string _expectedKeyId;

		[Given]
		public void WeHaveExpectedKeyId()
		{
			_expectedKeyId = "Test Key ID";
		}

		[Given]
		public void WeHaveProtectedData()
		{
			_protectedDataWithSegment = Convert.FromBase64String("CwAAAAACAABJd4EjRdkdFWyeo1Ff/3sJW5eSuFPC9GdZoFik6qUDQD3T5dwUT7Hx5EmdK8fzkgArxJR4TD+1HxomUfCa5JZlUwvP0D43tmXVC244/bTVmvSQwYkKwOqFtDLDLm52+ONGVuvxgRuXQwDQLc/cwdWzIRR5fmbpUcUl0EAHZ5E1iwW7qy2+xXPx8CEsJpuRKooLUP0Wye2mezbG4mcaMWfDDlpEE9yixiFzDaE9ZYxVqKCl17FmYnEcRi68D22CvgTFCAGQ2slI2TLN2R9jQKXorhrpCXpQull6kmLQ455M9bS8q7sOiqGJEIzt1evsl47gXUJljtXRzS3KMwq0qa4a6XuOZ3WR6BRVPJbcuHjPxDTTvXQJafdXBa8Fa1XspUh6bpIoLizuI7E5kwKJUene8ypBV4cQLoMZpOOQsQ3+h+ZouO7viDe5I+aF9k7bIhNYeKPDWzQrYgAdHqduf9ONZYbZD9n4J+YkG46J6S0wcd10YoMvjyytkvHSMXXMT/QGvr2Qo5FCQUy0nGx8tdX44UFA0LtKjmKF94CSUyihMFsXIcrfV+UiSa+Xff5I97rN9S9413B3vy59SjAnUqMFSkzADdUEY2RydgbMszVd5F+FN9YGvqAQaD41/PMNg67GISVBLL/eHZbjQQqVxjgTSWNYZOM2pGDQrBuTe67/MFRlc3QgS2V5IElEU2FtcGxlIERhdGEgVG8gUHJvdGVjdA==");
		}

		[When]
		public void WePrependKeyIdSegment()
		{
			_result = Manager.GetKeyId(_protectedDataWithSegment, out _keyIdSegmentSize);
		}

		[Then]
		public void ResultShouldBeReturned()
		{
			Assert.That(_result, Is.Not.Null);
		}

		[Then]
		public void ResultShouldMatchExpectedKey()
		{
			Assert.That(_result, Is.EqualTo(_expectedKeyId));
		}

		[Then]
		public void KeyIdSegmentSizeShouldBeCorrect()
		{
			Assert.That(_keyIdSegmentSize, Is.EqualTo(531));
		}
	}
}
