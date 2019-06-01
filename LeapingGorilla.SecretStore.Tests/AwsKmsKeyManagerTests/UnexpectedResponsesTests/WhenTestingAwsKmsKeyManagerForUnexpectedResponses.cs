using System.IO;
using Amazon.KeyManagementService;
using LeapingGorilla.SecretStore.Aws;
using LeapingGorilla.Testing;
using LeapingGorilla.Testing.Attributes;

namespace LeapingGorilla.SecretStore.Tests.AwsKmsKeyManagerTests.UnexpectedResponsesTests
{
	public abstract class WhenTestingAwsKmsKeyManagerForUnexpectedResponses : WhenTestingTheBehaviourOf
	{
		public string KeyId = File.ReadAllText("KmsTestKeyArn.txt");

		[ItemUnderTest]
		public AwsKmsKeyManager Manager { get; set; } 

		[Dependency]
		public IAmazonKeyManagementService KmsService { get; set; }
	}
}
