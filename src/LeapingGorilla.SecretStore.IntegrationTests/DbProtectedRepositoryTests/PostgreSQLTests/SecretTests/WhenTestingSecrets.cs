using System.Threading.Tasks;
using LeapingGorilla.SecretStore.Database.PostgreSQL;
using LeapingGorilla.Testing.Attributes;

namespace LeapingGorilla.SecretStore.IntegrationTests.DbProtectedRepositoryTests.PostgreSQLTests.SecretTests
{
	public abstract class WhenTestingSecrets : WhenTestingPostgreSQLDbRepository
	{
		protected override void CreateManualDependencies()
		{
			base.CreateManualDependencies();
			Repository = new PostgreSQLDbProtectedRepository(ReadOnlyConnectionString, ReadWriteConnectionString, TableName);
		}

		[Given(int.MinValue)]
		public async Task WeHaveCreatedTable()
		{
			await Repository.CreateProtectedSecretTableAsync(TableName);
		}
	}
}
