using System.Collections.Generic;
using System.Linq;
using LeapingGorilla.SecretStore.Tests.Builders;
using LeapingGorilla.Testing.Attributes;
using NSubstitute;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.CachingSecretStoreTests.CachingClearSecretStoreTests
{
	public class WhenCallingGetAllForApplicationInCache : WhenTestingCachingClearSecretStore
	{
		private IEnumerable<ClearSecret> _returnedSecrets;
		private IEnumerable<ClearSecret> _cachedSecrets;

		[Given]
		public void SecretInCache()
		{
			_cachedSecrets = new[]
			{
				ClearSecretBuilder.Create().AnInstance(),
				ClearSecretBuilder.Create().WithName("Test2").WithValue("Test2Value").AnInstance()
			};

			SecretCache.GetAllForApplication(clearSecret.ApplicationName).Returns(_cachedSecrets);
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
			Assert.That(_returnedSecrets, Has.Count.EqualTo(_cachedSecrets.Count()));
		}

		[Then]
		public void SecretsAsExpected()
		{
			Assert.That(_returnedSecrets, Is.EquivalentTo(_cachedSecrets));
		}
	}
}
