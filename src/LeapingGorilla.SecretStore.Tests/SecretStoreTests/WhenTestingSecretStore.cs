using LeapingGorilla.SecretStore.Interfaces;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit;

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
