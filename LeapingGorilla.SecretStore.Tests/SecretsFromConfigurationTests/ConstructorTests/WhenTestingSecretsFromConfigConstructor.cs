using LeapingGorilla.SecretStore.Interfaces;
using LeapingGorilla.Testing;
using LeapingGorilla.Testing.Attributes;

namespace LeapingGorilla.SecretStore.Tests.SecretsFromConfigurationTests.ConstructorTests
{
	public abstract class WhenTestingSecretsFromConfigConstructor : WhenTestingTheBehaviourOf
	{
		[Mock]
		public ISecretStore SecretStore { get; set; }
	}
}
