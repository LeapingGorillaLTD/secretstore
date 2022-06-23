using System.Collections.Generic;
using System.Linq;
using LeapingGorilla.SecretStore.Database.PostgreSQL;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit.Attributes;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.IntegrationTests.DbProtectedRepositoryTests.PostgreSQLTests.SecretTests
{
	public class WhenGettingAllForEmptyApplication : WhenTestingSecrets
	{
		private List<ProtectedSecret> returnedSecrets;

		protected override void CreateManualDependencies()
		{
			base.CreateManualDependencies();
			Repository = new PostgreSQLDbProtectedRepository(ReadOnlyConnectionString, ReadWriteConnectionString, TableName);
		}

		[When(DoNotRethrowExceptions: true)]
		public void WhenWeSaveThenRetrieveSecret()
		{
			returnedSecrets = Repository.GetAllForApplication("DoesNotExist").ToList();
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
