using LeapingGorilla.SecretStore.Caching;
using LeapingGorilla.SecretStore.Caching.Interfaces;
using LeapingGorilla.SecretStore.Interfaces;
using LeapingGorilla.SecretStore.Tests.Builders;
using LeapingGorilla.Testing;
using LeapingGorilla.Testing.Attributes;

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
