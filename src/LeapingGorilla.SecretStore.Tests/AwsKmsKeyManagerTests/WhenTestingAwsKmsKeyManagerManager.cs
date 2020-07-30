using LeapingGorilla.SecretStore.Aws;
using LeapingGorilla.Testing;
using LeapingGorilla.Testing.Attributes;

namespace LeapingGorilla.SecretStore.Tests.AwsKmsKeyManagerTests
{
	public abstract class WhenTestingAwsKmsKeyManagerManager : WhenTestingTheBehaviourOf
	{
		[ItemUnderTest]
		public AwsKmsKeyManager Manager { get; set; }

		[Dependency]
		public string RegionEndpoint { get; set; }

		protected override void CreateManualDependencies()
		{
			base.CreateManualDependencies();
			RegionEndpoint = "unknown";
		}
	}
}
