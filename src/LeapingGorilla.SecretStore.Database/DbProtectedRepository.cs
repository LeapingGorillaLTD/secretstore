﻿// /*
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

using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using LeapingGorilla.SecretStore.Exceptions;
using LeapingGorilla.SecretStore.Interfaces;

namespace LeapingGorilla.SecretStore.Database
{
	public abstract class DbProtectedRepository : IProtectedSecretRepository
	{
		protected readonly string readOnlyConnectionString;
		protected readonly string readWriteConnectionString;

		protected readonly string tableName;

		/// <summary>
		/// SQL that will be used to INSERT or UPDATE a given row. If your chosen
		/// DBMS does not support the concept of an all in one UPSERT you can override
		/// the Save/SaveAsync methods (or maybe you shouldn't inherit from this class)
		/// </summary>
		protected abstract string UpsertSql { get; }

		/// <summary>
		/// Creates a new DbProtectedRepository using the given DB Connection
		/// strings to access secrets. This variant allows for separating read
		/// and write commands to allow for the use of read replicas on high
		/// traffic clusters
		/// </summary>
		/// <param name="readOnlyConnectionString">
		/// Connection string to use for read-only commands (i.e. <see cref="Get"/>)
		/// </param>
		/// <param name="readWriteConnectionString">
		/// Connection string for commands requiring write access (i.e. <see cref="Save"/>)
		/// </param>
		/// <param name="tableName">
		/// Name of the table containing secrets. Note: This is set once on construction
		/// and is used in SQL throughout the class. You should be aware that this is a
		/// potential SQL injection risk - the table name should adhere to SQL standards
		/// as it will be interpolated in queries.
		/// </param>
		protected DbProtectedRepository(string readOnlyConnectionString, string readWriteConnectionString, string tableName)
		{
			this.readOnlyConnectionString = readOnlyConnectionString;
			this.readWriteConnectionString = readWriteConnectionString;
			this.tableName = tableName;
		}
		
		/// <summary>
		/// Creates a new DbProtectedRepository using the given DB Connection
		/// strings to access secrets. This variant takes a single connection string
		/// which should support both read and write commands
		/// </summary>
		/// <param name="readWriteConnectionString">
		/// Connection string for all commands requiring both read and write access
		/// (i.e. <see cref="Save"/>)
		/// </param>
		/// <param name="tableName">
		/// Name of the table containing secrets. Note: This is set once on construction
		/// and is used in SQL throughout the class. You should be aware that this is a
		/// potential SQL injection risk - the table name should adhere to SQL standards
		/// as it will be interpolated in queries.
		/// </param>
		protected DbProtectedRepository(string readWriteConnectionString, string tableName)
		{
			readOnlyConnectionString = readWriteConnectionString;
			this.readWriteConnectionString = readWriteConnectionString;
			this.tableName = tableName;
		}

		/// <inheritdoc />
		public ProtectedSecret Get(string applicationName, string secretName)
		{
			using var conn = CreateConnection();
			var secret = conn.QuerySingleOrDefault<ProtectedSecret>($"SELECT * FROM {tableName} WHERE applicationName=@applicationName AND secretName=@secretName",
				new { applicationName, secretName });

			return secret ?? throw new SecretNotFoundException(applicationName, secretName);
		}

		/// <inheritdoc />
		public async Task<ProtectedSecret> GetAsync(string applicationName, string secretName)
		{
			using var conn = CreateConnection();
			var secret = await conn.QuerySingleOrDefaultAsync<ProtectedSecret>($"SELECT * FROM {tableName} WHERE applicationName=@applicationName AND secretName=@secretName",
				new { applicationName, secretName });

			return secret ?? throw new SecretNotFoundException(applicationName, secretName);
		}

		/// <inheritdoc />
		public virtual void Save(ProtectedSecret secret)
		{
			using var conn = CreateConnection(requiresWrite: true);
			conn.Execute(UpsertSql, secret);
		}

		/// <inheritdoc />
		public virtual async Task SaveAsync(ProtectedSecret secret)
		{
			using var conn = CreateConnection(requiresWrite: true);
			await conn.ExecuteAsync(UpsertSql, secret);
		}

		/// <inheritdoc />
		public IEnumerable<ProtectedSecret> GetAllForApplication(string applicationName)
		{
			var sql = @$"SELECT * FROM {tableName} WHERE applicationName=@applicationName";
			
			using var conn = CreateConnection();
			return (conn.Query<ProtectedSecret>(sql, new { applicationName }) ?? Enumerable.Empty<ProtectedSecret>()).ToList();
		}

		/// <inheritdoc />
		public async Task<IEnumerable<ProtectedSecret>> GetAllForApplicationAsync(string applicationName)
		{
			var sql = @$"SELECT * FROM {tableName} WHERE applicationName=@applicationName";
			
			using var conn = CreateConnection();
			return (await conn.QueryAsync<ProtectedSecret>(sql, new { applicationName }) ?? Enumerable.Empty<ProtectedSecret>()).ToList();
		}

		/// <summary>
		/// Creates a Read/Write or ReadOnly connection depending on the
		/// <see cref="requiresWrite"/> parameter. The connection will be
		/// in a state where it is ready to run queries against the data source.
		/// </summary>
		/// <param name="requiresWrite">
		/// Does the query that this connection is being used for require write access?
		/// If true a ReadWrite connection will be provided, otherwise a Read Only
		/// connection will be returned.
		/// </param>
		/// <returns>
		/// A read only or read/write connection ready to be used to
		/// communicate with the DB
		/// </returns>
		protected abstract IDbConnection CreateConnection(bool requiresWrite = false);
	}
}