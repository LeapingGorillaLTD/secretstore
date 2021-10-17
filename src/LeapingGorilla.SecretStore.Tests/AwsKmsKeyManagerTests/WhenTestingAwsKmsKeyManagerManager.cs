using LeapingGorilla.SecretStore.Aws;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit;

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
