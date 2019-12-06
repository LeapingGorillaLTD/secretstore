using System.Threading.Tasks;
using LeapingGorilla.Testing.Attributes;
using NSubstitute;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.CachingSecretStoreTests.CachingClearSecretStoreTests
{
	public class WhenCallingGetAsyncInCache : WhenTestingCachingClearSecretStore
	{
		private ClearSecret _returnedSecret;

		[Given]
		public void SecretNotInCache()
		{
			SecretCache
				.GetAsync(clearSecret.ApplicationName, clearSecret.Name)
				.Returns(Task.FromResult(clearSecret));
		}

		[When]
		public async Task GetAsyncCalled()
		{
			_returnedSecret = await SecretStore.GetAsync(clearSecret.ApplicationName, clearSecret.Name);
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
			SecretRepository.Received(0).GetAsync(Arg.Any<string>(), Arg.Any<string>());
		}
	}
}
