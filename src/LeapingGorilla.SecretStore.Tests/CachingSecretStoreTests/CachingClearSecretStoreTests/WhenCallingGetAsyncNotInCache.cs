using System.Threading.Tasks;
using LeapingGorilla.Testing.Attributes;
using NSubstitute;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.CachingSecretStoreTests.CachingClearSecretStoreTests
{
	public class WhenCallingGetAsyncNotInCache : WhenTestingCachingClearSecretStore
	{
		private ClearSecret _returnedSecret;

		[Given]
		public void SecretNotInCache()
		{
			SecretCache
				.GetAsync(Arg.Any<string>(), Arg.Any<string>())
				.Returns(Task.FromResult(default(ClearSecret)));
		}

		[Given]
		public void SecretRepoGetReturnsProtectedSecret()
		{
			SecretRepository.GetAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(Task.FromResult(protectedSecret));
		}

		[Given]
		public void DecryptReturnsClearValue()
		{
			EncryptionManager
				.Decrypt(Arg.Any<string>(), Arg.Any<byte[]>(), Arg.Any<byte[]>(), Arg.Any<byte[]>())
				.Returns(BaseSecretStore.SecretEncoding.GetBytes(clearSecret.Value));
		}


		[When]
		public async Task GetCalled()
		{
			_returnedSecret = await SecretStore.GetAsync(clearSecret.ApplicationName, clearSecret.Name);
		}

		[Then]
		public void SecretReturned()
		{
			Assert.That(_returnedSecret, Is.Not.Null);
		}

		[Then]
		public void SecretAddedToCache()
		{
			SecretCache.Received(1).AddAsync(_returnedSecret);
		}
	}
}
