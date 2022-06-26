// /*
//    Copyright 2013-2022 Leaping Gorilla LTD
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//        http://www.apache.org/licenses/LICENSE-2.0
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// */

using System.Collections.Generic;
using System.Linq;
using LeapingGorilla.SecretStore.Tests.Builders;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit.Attributes;
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
