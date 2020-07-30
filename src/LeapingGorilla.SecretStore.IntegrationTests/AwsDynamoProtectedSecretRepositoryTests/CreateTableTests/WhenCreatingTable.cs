using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using FluentAssertions;
using FluentAssertions.Extensions;
using LeapingGorilla.SecretStore.Tests.Builders;
using LeapingGorilla.Testing.Attributes;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.IntegrationTests.AwsDynamoProtectedSecretRepositoryTests.CreateTableTests
{
	public class WhenCreatingTable : WhenTestingAwsDynamoProtectedSecretRepository
	{
		private ProtectedSecret _savedSecret;
		private ProtectedSecret _retrievedSecret;

		[When]
		public async Task TableCreatedAndSecretAdded()
		{
			await Repository.CreateProtectedSecretTableAsync(TableName);
			_savedSecret = ProtectedSecretBuilder.Create().AnInstance();
			await Repository.SaveAsync(_savedSecret);
			_retrievedSecret = await Repository.GetAsync(_savedSecret.ApplicationName, _savedSecret.Name);
		}

		[Then]
		public void SecretShouldBeRetrieved()
		{
			Assert.That(_retrievedSecret, Is.Not.Null);
		}

		[Then]
		public void RetrievedSecretShouldBeAsExpected()
		{
			_retrievedSecret.Should().BeEquivalentTo(_savedSecret);
		}

		[OneTimeTearDown]
		public void DeleteTable()
		{
			var client = new AmazonDynamoDBClient(RegionEndpoint.EUWest1);
			client.DeleteTableAsync(TableName).Wait(30.Seconds());
		}
	}
}
