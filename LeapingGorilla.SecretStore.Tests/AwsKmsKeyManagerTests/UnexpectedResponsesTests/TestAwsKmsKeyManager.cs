using Amazon.KeyManagementService;
using LeapingGorilla.SecretStore.Aws;

namespace LeapingGorilla.SecretStore.Tests.AwsKmsKeyManagerTests.UnexpectedResponsesTests
{
	public class TestAwsKmsKeyManager : AwsKmsKeyManager
	{
		private readonly IAmazonKeyManagementService _kmsService;

		protected override IAmazonKeyManagementService CreateClient()
		{
			return _kmsService;
		}

		public TestAwsKmsKeyManager(IAmazonKeyManagementService service) : base("unknown")
		{
			_kmsService = service;
		}
	}
}
