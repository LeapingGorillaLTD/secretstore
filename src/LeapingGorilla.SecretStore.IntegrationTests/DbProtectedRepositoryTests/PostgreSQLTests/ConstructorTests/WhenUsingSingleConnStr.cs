using System.Threading.Tasks;
using LeapingGorilla.SecretStore.Database.PostgreSQL;
using LeapingGorilla.SecretStore.Tests.Builders;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit.Attributes;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.IntegrationTests.DbProtectedRepositoryTests.PostgreSQLTests.ConstructorTests
{
	public class WhenUsingSingleConnStr : WhenTestingPostgreSQLDbRepository
	{
		private ProtectedSecret savedSecret;
		private ProtectedSecret returnedSecret;

		protected override void CreateManualDependencies()
		{
			base.CreateManualDependencies();
			Repository = new PostgreSQLDbProtectedRepository(ReadWriteConnectionString, TableName);
		}

		[Given]
		public void WeHaveSecretToSave()
		{
			savedSecret = ProtectedSecretBuilder.Create().AnInstance();
		}

		[When(DoNotRethrowExceptions: true)]
		public async Task WhenWeSaveThenRetrieveSecret()
		{
			await Repository.CreateProtectedSecretTableAsync(TableName);
			Repository.Save(savedSecret);
			returnedSecret = Repository.Get(savedSecret.ApplicationName, savedSecret.Name);
		}

		[Then]
		public void NoExceptionThrown()
		{
			Assert.That(ThrownException, Is.Null);
		}

		[Then]
		public void ReturnedSecretShouldHaveExpectedSecretValue()
		{
			Assert.That(returnedSecret.ProtectedSecretValue, Is.EqualTo(savedSecret.ProtectedSecretValue));
		}
		
		[Then]
		public void ReturnedSecretShouldHaveExpectedIv()
		{
			Assert.That(returnedSecret.InitialisationVector, Is.EqualTo(savedSecret.InitialisationVector));
		}
		
		[Then]
		public void ReturnedSecretShouldHaveExpectedDocumentKey()
		{
			Assert.That(returnedSecret.ProtectedDocumentKey, Is.EqualTo(savedSecret.ProtectedDocumentKey));
		}
		
		[Then]
		public void ReturnedSecretShouldHaveExpectedMasterKey()
		{
			Assert.That(returnedSecret.MasterKeyId, Is.EqualTo(savedSecret.MasterKeyId));
		}
	}
}
