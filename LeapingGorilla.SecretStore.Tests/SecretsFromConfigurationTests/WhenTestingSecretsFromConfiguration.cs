using LeapingGorilla.SecretStore.Configuration;
using LeapingGorilla.SecretStore.Interfaces;
using LeapingGorilla.Testing;
using LeapingGorilla.Testing.Attributes;

namespace LeapingGorilla.SecretStore.Tests.SecretsFromConfigurationTests
{
	public abstract class WhenTestingSecretsFromConfiguration : WhenTestingTheBehaviourOf
	{
		protected const string Success = "SUCCESS";
		protected static readonly Secret SuccessSecret = new Secret { SecretName = "SuccessSecret", Value = Success };

		[ItemUnderTest]
		public SecretsFromConfiguration SecretsConfig { get; set; }

		[Dependency]
		public ISecretStore SecretStore { get; set; }
	}
}
