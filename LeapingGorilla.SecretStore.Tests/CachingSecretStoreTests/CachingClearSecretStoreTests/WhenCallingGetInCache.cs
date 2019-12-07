using LeapingGorilla.Testing.Attributes;
using NSubstitute;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.CachingSecretStoreTests.CachingClearSecretStoreTests
{
	public class WhenCallingGetInCache : WhenTestingCachingClearSecretStore
	{
		private ClearSecret _returnedSecret;

		[Given]
		public void SecretInCache()
		{
			SecretCache.Get(clearSecret.ApplicationName, clearSecret.Name).Returns(clearSecret);
		}

		[When]
		public void GetCalled()
		{
			_returnedSecret = SecretStore.Get(clearSecret.ApplicationName, clearSecret.Name);
		}

		[Then]
		public void SecretReturned()
		{
			Assert.That(_returnedSecret, Is.Not.Null);
		}

		[Then]
		public void SecretAsExpected()
		{
			Assert.That(_returnedSecret, Is.EqualTo(clearSecret));
		}

		[Then]
		public void SecretRepoGetNotCalled()
		{
			SecretRepository.Received(0).Get(Arg.Any<string>(), Arg.Any<string>());
		}
	}
}
