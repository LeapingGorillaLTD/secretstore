using Amazon.KeyManagementService;
using LeapingGorilla.Testing;
using LeapingGorilla.Testing.Attributes;

namespace LeapingGorilla.SecretStore.Tests.AwsKmsKeyManagerTests.UnexpectedResponsesTests
{
	public abstract class WhenTestingAwsKmsKeyManagerForUnexpectedResponses : WhenTestingTheBehaviourOf
	{
		public const string KeyId = "Test";

		[ItemUnderTest]
		public TestAwsKmsKeyManager Manager { get; set; } 

		[Dependency]
		public IAmazonKeyManagementService KmsService { get; set; }
	}
}
