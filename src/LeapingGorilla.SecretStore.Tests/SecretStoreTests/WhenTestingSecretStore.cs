using LeapingGorilla.SecretStore.Interfaces;
using LeapingGorilla.Testing;
using LeapingGorilla.Testing.Attributes;

namespace LeapingGorilla.SecretStore.Tests.SecretStoreTests
{
	public abstract class WhenTestingSecretStore : WhenTestingTheBehaviourOf
	{
		[ItemUnderTest]
		public SecretStore SecretStore { get; set; }

		[Dependency]
		public IProtectedSecretRepository SecretRepository { get; set; }

		[Dependency]
		public IEncryptionManager EncryptionManager { get; set; }
	}
}
