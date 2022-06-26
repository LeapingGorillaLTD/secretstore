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

using LeapingGorilla.SecretStore.Caching;
using LeapingGorilla.SecretStore.Caching.Interfaces;
using LeapingGorilla.SecretStore.Interfaces;
using LeapingGorilla.SecretStore.Tests.Builders;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit;

namespace LeapingGorilla.SecretStore.Tests.CachingSecretStoreTests.CachingClearSecretStoreTests
{
	public abstract class WhenTestingCachingClearSecretStore : WhenTestingTheBehaviourOf
	{
		protected ClearSecret clearSecret = ClearSecretBuilder.Create().AnInstance();
		protected ProtectedSecret protectedSecret = ProtectedSecretBuilder.Create().AnInstance();
		protected string keyName = ProtectedSecretBuilder.Defaults.MasterKeyId;

		[ItemUnderTest]
		public CachingClearSecretStore SecretStore { get; set; }

		[Dependency]
		public IProtectedSecretRepository SecretRepository { get; set; }

		[Dependency]
		public IEncryptionManager EncryptionManager { get; set; }

		[Dependency]
		public ISecretCache<ClearSecret> SecretCache { get; set; }
	}
}
