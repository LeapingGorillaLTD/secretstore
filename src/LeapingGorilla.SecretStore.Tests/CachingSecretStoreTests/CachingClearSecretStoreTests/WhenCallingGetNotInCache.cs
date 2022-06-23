using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit.Attributes;
using NSubstitute;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.CachingSecretStoreTests.CachingClearSecretStoreTests
{
	public class WhenCallingGetNotInCache : WhenTestingCachingClearSecretStore
	{
		private ClearSecret _returnedSecret;

		[Given]
		public void SecretNotInCache()
		{
			SecretCache.Get(Arg.Any<string>(), Arg.Any<string>()).Returns(default(ClearSecret));
		}

		[Given]
		public void SecretRepoGetReturnsProtectedSecret()
		{
			SecretRepository.Get(Arg.Any<string>(), Arg.Any<string>()).Returns(protectedSecret);
		}

		[Given]
		public void DecryptReturnsClearValue()
		{
			EncryptionManager
				.Decrypt(Arg.Any<string>(), Arg.Any<byte[]>(), Arg.Any<byte[]>(), Arg.Any<byte[]>())
				.Returns(BaseSecretStore.SecretEncoding.GetBytes(clearSecret.Value));
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
		public void SecretAddedToCache()
		{
			SecretCache.Received(1).Add(_returnedSecret);
		}
	}
}
