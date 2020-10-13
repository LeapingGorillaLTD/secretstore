using System.Data;
using System.Threading.Tasks;
using Dapper;
using LeapingGorilla.SecretStore.Interfaces;
using Npgsql;

namespace LeapingGorilla.SecretStore.Database.PostgreSQL
{
	public class PostgreSQLDbProtectedRepository : DbProtectedRepository, ICreateProtectedSecretTable
	{
		public PostgreSQLDbProtectedRepository(string readOnlyConnectionString, string readWriteConnectionString, string tableName) 
			: base(readOnlyConnectionString, readWriteConnectionString, tableName)
		{
		}

		public PostgreSQLDbProtectedRepository(string readWriteConnectionString, string tableName) 
			: base(readWriteConnectionString, tableName)
		{
		}

		protected override IDbConnection CreateConnection(bool requiresWrite = false)
		{
			return requiresWrite
				? new NpgsqlConnection(readWriteConnectionString)
				: new NpgsqlConnection(readOnlyConnectionString);
		}

		public async Task CreateProtectedSecretTableAsync(string tableName)
		{
			var sql = $@"CREATE TABLE {tableName} (
							ApplicationName TEXT NOT NULL,
							SecretName TEXT NOT NULL,
							InitialisationVector BYTEA NOT NULL,
							MasterKeyId TEXT NOT NULL,
							ProtectedDocumentKey BYTEA NOT NULL,
							ProtectedSecretValue BYTEA NOT NULL,
							PRIMARY KEY(ApplicationName, SecretName)
						);";

			using var conn = CreateConnection(requiresWrite: true);
			await conn.ExecuteAsync(sql);
		}
	}
}
