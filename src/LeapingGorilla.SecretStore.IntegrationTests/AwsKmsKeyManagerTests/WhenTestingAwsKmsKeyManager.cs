using System.IO;
using LeapingGorilla.SecretStore.Aws;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit;

namespace LeapingGorilla.SecretStore.IntegrationTests.AwsKmsKeyManagerTests
{
	public abstract class WhenTestingAwsKmsKeyManager : WhenTestingTheBehaviourOf
	{
		protected readonly string DevelopmentKeyId = File.ReadAllText("KmsTestKeyArn.txt");

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