using System.IO;
using LeapingGorilla.SecretStore.Aws;
using LeapingGorilla.Testing;
using LeapingGorilla.Testing.Attributes;

namespace LeapingGorilla.SecretStore.IntegrationTests.AwsKmsKeyManagerTests
{
	public abstract class WhenTestingAwsKmsKeyManager : WhenTestingTheBehaviourOf
	{
		public string DevelopmentKeyId = File.ReadAllText("KmsTestKeyArn.txt");

		[ItemUnderTest]
		public AwsKmsKeyManager Manager { get; set; } 

		[Dependency]
		public string RegionEndpoint { get; set; }

		protected override void CreateManualDependencies()
		{
			base.CreateManualDependencies();
			RegionEndpoint = "eu-west-1";
		}
	}
}
