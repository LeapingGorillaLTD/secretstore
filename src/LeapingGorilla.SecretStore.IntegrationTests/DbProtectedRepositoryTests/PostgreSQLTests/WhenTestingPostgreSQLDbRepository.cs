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
		protected string DbName;

		private readonly string roUsername = "ss_integration_test_ro";
		private readonly string rwUsername = "ss_integration_test";

		public PostgreSQLDbProtectedRepository Repository { get; set; }

		protected override void CreateManualDependencies()
		{
			base.CreateManualDependencies();

			DbName = $"SecretStore_IntegrationTests".ToLowerInvariant();
			var connStr = File.ReadAllText("PostgreSQLConnectionString.txt")?.Trim()
			              ?? throw new ApplicationException("Failed to find a connection string in PostgreSQLConnectionString.txt");

			using var conn = new NpgsqlConnection(connStr);
			CleanupDb(conn);

			var roPassword = "concise-cornfield-celery-defiling-brethren-drizzle";
			var rwPassword = "managing-octane-job-rebate-scolding-schematic";

			// Determine the RO/RW connection strings
			var roConnStr = new NpgsqlConnectionStringBuilder(conn.ConnectionString)
			{
				Database = DbName, 
				Username = roUsername, 
				Password = roPassword
			};
			ReadOnlyConnectionString = roConnStr.ToString();
			
			var rwConnStr = new NpgsqlConnectionStringBuilder(conn.ConnectionString)
			{
				Database = DbName, 
				Username = rwUsername, 
				Password = rwPassword
			};
			ReadWriteConnectionString = rwConnStr.ToString();
		}

		[OneTimeTearDown]
		public void DropDb()
		{
			using var conn = new NpgsqlConnection(ReadWriteConnectionString);
			CleanupDb(conn);
		}

		private void CleanupDb(IDbConnection conn)
		{
			// Clean up existing DB or roles
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
