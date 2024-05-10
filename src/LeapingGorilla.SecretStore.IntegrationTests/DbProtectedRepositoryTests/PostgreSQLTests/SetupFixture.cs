// /*
//    Copyright 2013-2024 Leaping Gorilla LTD
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

using System;
using System.IO;
using System.Reflection;
using System.Threading;
using Dapper;
using DotNet.Testcontainers.Builders;
using Npgsql;
using Testcontainers.PostgreSql;

namespace LeapingGorilla.SecretStore.IntegrationTests.DbProtectedRepositoryTests.PostgreSQLTests;

[SetUpFixture]
public class SetupFixture
{
    private PostgreSqlContainer _databaseContainer;

    private const string DbPassword = "plaything-osmosis-calzone-reiterate-pesticide-hazelnut";
    
    /// These passwords should match what is in initdb.sh. If you alter the init script then alter these as well
    private const string RoDbPassword = "concise-cornfield-celery-defiling-brethren-drizzle";
    private const string RwDbPassword = "managing-octane-job-rebate-scolding-schematic";

    public static string ReadOnlyConnectionString { get; private set; }
    
    public static string ReadWriteConnectionString { get; private set; }
    
    
    [OneTimeSetUp]
    public async Task PrepareContainer()
    {
        Console.WriteLine("Starting container");
        using var startupTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(2));
        _databaseContainer = new PostgreSqlBuilder()
            .WithPassword(DbPassword)
            .WithImage("postgres:16")
            .Build();
        await _databaseContainer.StartAsync(startupTokenSource.Token);
        Console.WriteLine("Container started");

        var connStrBuilder = new NpgsqlConnectionStringBuilder(_databaseContainer.GetConnectionString());
        using (var conn = new NpgsqlConnection(connStrBuilder.ToString()))
        {
            await conn.ExecuteAsync(
@$"CREATE USER ss_integration_test_ro WITH ENCRYPTED PASSWORD '{RoDbPassword}';
CREATE USER ss_integration_test WITH ENCRYPTED PASSWORD '{RwDbPassword}';
GRANT ss_integration_test_ro TO ss_integration_test");

            // "CREATE DATABASE cannot be executed within a pipeline" - Move create DB to its own execution
            await conn.ExecuteAsync(@"CREATE DATABASE ss_integration_test_db WITH ENCODING 'UTF8' OWNER ss_integration_test;");
            await conn.ExecuteAsync(@"ALTER DATABASE ss_integration_test_db SET TIMEZONE='UTC';");
        }

        connStrBuilder.Database = "ss_integration_test_db";
        using (var conn = new NpgsqlConnection(connStrBuilder.ToString()))
        {
            await conn.ExecuteAsync(
@"GRANT CONNECT ON DATABASE ss_integration_test_db TO ss_integration_test_ro; -- RW will inherit
GRANT TEMP ON DATABASE ss_integration_test_db TO ss_integration_test_ro;

GRANT ALL PRIVILEGES ON DATABASE ss_integration_test_db TO ss_integration_test WITH GRANT OPTION;


GRANT USAGE ON SCHEMA public TO ss_integration_test_ro;
ALTER DEFAULT PRIVILEGES FOR USER ss_integration_test IN SCHEMA public GRANT SELECT,REFERENCES     ON TABLES       TO ss_integration_test_ro;
ALTER DEFAULT PRIVILEGES FOR USER ss_integration_test IN SCHEMA public GRANT ALL                   ON SEQUENCES    TO ss_integration_test_ro;
ALTER DEFAULT PRIVILEGES FOR USER ss_integration_test IN SCHEMA public GRANT EXECUTE               ON FUNCTIONS    TO ss_integration_test_ro;
ALTER DEFAULT PRIVILEGES FOR USER ss_integration_test IN SCHEMA public GRANT USAGE                 ON TYPES        TO ss_integration_test_ro;

REVOKE ALL ON DATABASE ss_integration_test_db FROM public;");
        }
        
        connStrBuilder.Username = "ss_integration_test_ro";
        connStrBuilder.Password = RoDbPassword;
        ReadOnlyConnectionString = connStrBuilder.ToString();
        
        connStrBuilder.Username = "ss_integration_test";
        connStrBuilder.Password = RwDbPassword;
        ReadWriteConnectionString = connStrBuilder.ToString();
    }

    [OneTimeTearDown]
    public async Task CleanupContainer()
    {
        using var shutDownTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(2));
        await _databaseContainer.StopAsync(shutDownTokenSource.Token);
    }
}