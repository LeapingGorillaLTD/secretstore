using Amazon;
using Amazon.DynamoDBv2;
using LeapingGorilla.SecretStore.Aws;
using LeapingGorilla.Testing;

namespace LeapingGorilla.SecretStore.IntegrationTests.AwsDynamoProtectedSecretRepositoryTests
{
	public abstract class WhenTestingAwsDynamoProtectedSecretRepository : WhenTestingTheBehaviourOf
	{
		public string TableName  => "TestingAwsDynamoProtectedSecretRepository";

		public AmazonDynamoDBConfig Config => new AmazonDynamoDBConfig { RegionEndpoint = RegionEndpoint.EUWest1 };

		public AwsDynamoProtectedSecretRepository Repository => new AwsDynamoProtectedSecretRepository(Config,  TableName);
	}
}
