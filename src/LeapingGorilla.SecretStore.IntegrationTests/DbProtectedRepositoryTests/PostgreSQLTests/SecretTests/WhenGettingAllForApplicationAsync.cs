using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeapingGorilla.SecretStore.Database.PostgreSQL;
using LeapingGorilla.SecretStore.Tests.Builders;
using LeapingGorilla.Testing.Attributes;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.IntegrationTests.DbProtectedRepositoryTests.PostgreSQLTests.SecretTests
{
	public class WhenGettingAllForApplicationAsync : WhenTestingSecrets
	{
		private ProtectedSecret firstSecret;
		private ProtectedSecret secondSecret;
		private ProtectedSecret notReturnedSecret;
		private List<ProtectedSecret> returnedSecrets;

		protected override void CreateManualDependencies()
		{
			base.CreateManualDependencies();
			Repository = new PostgreSQLDbProtectedRepository(ReadOnlyConnectionString, ReadWriteConnectionString, TableName);
		}

		[Given]
		public void WeHaveSecretsToSave()
		{
			firstSecret = ProtectedSecretBuilder.Create().AnInstance();
			secondSecret = ProtectedSecretBuilder
				.Create()
				.WithName("SecondSecret")
				.AnInstance();
			
			notReturnedSecret = ProtectedSecretBuilder
				.Create()
				.WithApplicationName("DifferentApp")
				.WithName("NotReturned")
				.AnInstance();
		}

		[When(DoNotRethrowExceptions: true)]
		public async Task WhenWeSaveThenRetrieveSecret()
		{
			await Repository.SaveAsync(firstSecret);
			await Repository.SaveAsync(secondSecret);
			await Repository.SaveAsync(notReturnedSecret);
			returnedSecrets = (await Repository.GetAllForApplicationAsync(firstSecret.ApplicationName)).ToList();
		}

		[Then]
		public void NoExceptionThrown()
		{
			Assert.That(ThrownException, Is.Null);
		}

		[Then]
		public void WeHaveSecrets()
		{
			Assert.That(returnedSecrets, Is.Not.Null);
		}

		[Then]
		public void WeShouldHaveExpectedNumberOfSecrets()
		{
			Assert.That(returnedSecrets, Has.Count.EqualTo(2));
		}
	}
}
