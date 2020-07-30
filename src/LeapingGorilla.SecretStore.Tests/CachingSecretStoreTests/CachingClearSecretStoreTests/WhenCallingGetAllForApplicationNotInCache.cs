using System.Collections.Generic;
using System.Linq;
using LeapingGorilla.SecretStore.Tests.Builders;
using LeapingGorilla.Testing.Attributes;
using NSubstitute;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.CachingSecretStoreTests.CachingClearSecretStoreTests
{
	public class WhenCallingGetAllForApplicationNotInCache : WhenTestingCachingClearSecretStore
	{
		private IEnumerable<ClearSecret> _returnedSecrets;
		private IEnumerable<ProtectedSecret> _allSecrets;
		private IEnumerable<ClearSecret> _cachedSecrets;

		[Given]
		public void SecretNotInCache()
		{
			SecretCache.GetAllForApplication(clearSecret.ApplicationName).Returns(default(IEnumerable<ClearSecret>));
		}

		[Given]
		public void ProtectedSecretsAvailable()
		{
			_allSecrets = new[]
			{
				ProtectedSecretBuilder.Create().AnInstance(),
				ProtectedSecretBuilder.Create().WithName("Test2").AnInstance()
			};

			SecretRepository.GetAllForApplication(clearSecret.ApplicationName).Returns(_allSecrets);
		}

		[Given]
		public void RecordCachedSecrets()
		{
			SecretCache
				.When(sc => sc.AddMany(Arg.Any<IEnumerable<ClearSecret>>()))
				.Do(ci => _cachedSecrets = ci.Arg<IEnumerable<ClearSecret>>());
		}

		[When]
		public void GetAllForApplication()
		{
			_returnedSecrets = SecretStore.GetAllForApplication(clearSecret.ApplicationName);
		}

		[Then]
		public void SecretsReturned()
		{
			Assert.That(_returnedSecrets, Is.Not.Null);
		}

		[Then]
		public void ExpectedNumberOfSecretsReturned()
		{
			Assert.That(_returnedSecrets, Has.Count.EqualTo(_allSecrets.Count()));
		}

		[Then]
		public void SecretsWereCached()
		{
			Assert.That(_cachedSecrets, Is.Not.Null);
		}

		[Then]
		public void CachedSecretsMatchReturned()
		{
			Assert.That(_cachedSecrets, Is.EquivalentTo(_returnedSecrets));
		}
	}
}
