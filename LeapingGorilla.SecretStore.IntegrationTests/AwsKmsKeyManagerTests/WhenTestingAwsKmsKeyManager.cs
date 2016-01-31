using LeapingGorilla.SecretStore.Aws;
using LeapingGorilla.Testing;
using LeapingGorilla.Testing.Attributes;

namespace LeapingGorilla.SecretStore.IntegrationTests.AwsKmsKeyManagerTests
{
	public abstract class WhenTestingAwsKmsKeyManager : WhenTestingTheBehaviourOf
	{
		public const string DevelopmentKeyId = "arn:aws:kms:eu-west-1:147007673657:key/36e5afca-a537-4cad-8b84-442e195c5d19";

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
