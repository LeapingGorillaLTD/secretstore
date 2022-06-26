// /*
//    Copyright 2013-2022 Leaping Gorilla LTD
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//        http://www.apache.org/licenses/LICENSE-2.0
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// */

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