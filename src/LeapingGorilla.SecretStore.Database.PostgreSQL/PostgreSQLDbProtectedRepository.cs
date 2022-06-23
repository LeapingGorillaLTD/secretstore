using System.Data;
using System.Threading.Tasks;
using Dapper;
using LeapingGorilla.SecretStore.Interfaces;
using Npgsql;

namespace LeapingGorilla.SecretStore.Database.PostgreSQL
{
	public class PostgreSQLDbProtectedRepository : DbProtectedRepository, ICreateProtectedSecretTable
	{
		private string PrimaryKeyName => @$"""pk_{tableName}:ApplicationName-SecretName""";

		public PostgreSQLDbProtectedRepository(string readOnlyConnectionString, string readWriteConnectionString, string tableName) 
			: base(readOnlyConnectionString, readWriteConnectionString, tableName)
		{
		}

		public PostgreSQLDbProtectedRepository(string readWriteConnectionString, string tableName) 
			: base(readWriteConnectionString, tableName)
		{
		}

		/// <inheritdoc cref="DbProtectedRepository.UpsertSql"/>
		protected override string UpsertSql => 
			@$"INSERT INTO {tableName}
				(
					ApplicationName, 
					SecretName, 
					InitialisationVector, 
					MasterKeyId, 
					ProtectedDocumentKey, 
					ProtectedSecretValue
				)
				VALUES
				(
					@ApplicationName, 
					@Name, 
					@InitialisationVector, 
					@MasterKeyId, 
					@ProtectedDocumentKey, 
					@ProtectedSecretValue
				)
				ON CONFLICT ON CONSTRAINT {PrimaryKeyName}
				DO
				UPDATE SET 
					initialisationVector=EXCLUDED.InitialisationVector, 
					masterKeyId=EXCLUDED.MasterKeyId, 
					protectedDocumentKey=EXCLUDED.ProtectedDocumentKey,
					protectedSecretValue=EXCLUDED.ProtectedSecretValue;";

		/// <inheritdoc />
		protected override IDbConnection CreateConnection(bool requiresWrite = false)
		{
			return requiresWrite
				? new NpgsqlConnection(readWriteConnectionString)
				: new NpgsqlConnection(readOnlyConnectionString);
		}

		/// <inheritdoc />
		public async Task CreateProtectedSecretTableAsync(string secretTableName)
		{
			var sql = $@"CREATE TABLE {secretTableName} (
							ApplicationName TEXT NOT NULL,
							SecretName TEXT NOT NULL,
							InitialisationVector BYTEA NOT NULL,
							MasterKeyId TEXT NOT NULL,
							ProtectedDocumentKey BYTEA NOT NULL,
							ProtectedSecretValue BYTEA NOT NULL,
							CONSTRAINT {PrimaryKeyName} PRIMARY KEY (ApplicationName, SecretName)
						);";

			using var conn = CreateConnection(requiresWrite: true);
			await conn.ExecuteAsync(sql);
		}
	}
}