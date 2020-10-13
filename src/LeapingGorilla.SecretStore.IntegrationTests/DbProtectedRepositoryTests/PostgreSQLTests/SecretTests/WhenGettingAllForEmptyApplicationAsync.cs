using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeapingGorilla.SecretStore.Database.PostgreSQL;
using LeapingGorilla.Testing.Attributes;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.IntegrationTests.DbProtectedRepositoryTests.PostgreSQLTests.SecretTests
{
	public class WhenGettingAllForEmptyApplicationAsync : WhenTestingSecrets
	{
		private List<ProtectedSecret> returnedSecrets;

		protected override void CreateManualDependencies()
		{
			base.CreateManualDependencies();
			Repository = new PostgreSQLDbProtectedRepository(ReadOnlyConnectionString, ReadWriteConnectionString, TableName);
		}

		[When(DoNotRethrowExceptions: true)]
		public async Task WhenWeSaveThenRetrieveSecret()
		{
			returnedSecrets = (await Repository.GetAllForApplicationAsync("DoesNotExist")).ToList();
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
			Assert.That(returnedSecrets, Is.Empty);
		}
	}
}
