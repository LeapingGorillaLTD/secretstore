using System;
using System.Data;
using System.IO;
using Dapper;
using LeapingGorilla.SecretStore.Database.PostgreSQL;
using LeapingGorilla.Testing.NUnit;
using Npgsql;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.IntegrationTests.DbProtectedRepositoryTests.PostgreSQLTests
{
	[SingleThreaded]
	public abstract class WhenTestingPostgreSQLDbRepository : WhenTestingTheBehaviourOf
	{
		protected string ReadOnlyConnectionString;
		protected string ReadWriteConnectionString;

        protected readonly string TableName = "SecretStore_IntegrationTest" + Guid.NewGuid().ToString("N");
		private const string DbName = "ss_integration_test_db";

		private readonly string roUsername = "ss_integration_test_ro";
		private readonly string rwUsername = "ss_integration_test";

		public PostgreSQLDbProtectedRepository Repository { get; set; }

		protected override void CreateManualDependencies()
		{
			base.CreateManualDependencies();
			
			// Determine the RO/RW connection strings
            var roConnStr = new NpgsqlConnectionStringBuilder()
            {
	            Host = "localhost",
	            Port = 54340,
                Database = DbName, 
                Username = roUsername, 
                Password = "concise-cornfield-celery-defiling-brethren-drizzle"
            };
            
            var rwConnStr = new NpgsqlConnectionStringBuilder()
            {
	            Host = "localhost",
	            Port = 54340,
	            Database = DbName, 
	            Username = rwUsername, 
	            Password = "managing-octane-job-rebate-scolding-schematic"
            };

            ReadOnlyConnectionString = roConnStr.ToString();
            ReadWriteConnectionString = rwConnStr.ToString();

            CleanupDb();
		}

		[OneTimeTearDown]
		public void CleanupDb()
		{
			using var conn = new NpgsqlConnection(ReadWriteConnectionString);
			CleanupDb(conn);
		}

		private void CleanupDb(IDbConnection conn)
		{
			// Clean up existing DB
			try
			{
				conn.Execute(@$"DROP TABLE IF EXISTS {TableName};");
			}
			catch (Exception ex)
			{
				Console.WriteLine("Failure dropping table, ignoring - {0}", ex);
			}
		}
	}
}