using LeapingGorilla.SecretStore.Aws;
using LeapingGorilla.Testing;
using LeapingGorilla.Testing.Attributes;

namespace LeapingGorilla.SecretStore.Tests.AwsKmsKeyManagerTests
{
	public abstract class WhenTestingAwsKmsKeyManagerManager : WhenTestingTheBehaviourOf
	{
		[ItemUnderTest]
		public AwsKmsKeyManager Manager { get; set; }
	}
}
