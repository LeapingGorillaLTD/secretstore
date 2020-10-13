# Running Integration Tests

Integration tests by definition rely on having access to external services. For SecretStore our integration tests need to access AWS and a Postgres Database. We use files in the root of the solution to contain configuration of these items so taht we do not commit sensitive information to the repository.

# Getting Started

Tell Git that you don't want to record changes to configuration files containing potentially sensitive information.

`git update-index --assume-unchanged src/LeapingGorilla.SecretStore.IntegrationTests/KmsTestKeyArn.txt`
`git update-index --assume-unchanged src/LeapingGorilla.SecretStore.IntegrationTests/PostgreSQLConnectionString.txt`

## Database Tests

1. Create a database to contain tables for your integration tests. You can use the `CreateIntegrationTestsDb.sql` scripts to make this easier.

2. Add a connection string to the `PostgreSQLConnectionString.txt` file. This should be on a single line and should have permissions to connect to the database and delete tables. This should look something like:

`Server=SQL01;Port=5432;Database=secretstore_integrationtests;User Id=ssuser;Password=XXX;`

3. Run the tests

## AWS Tests

1. Create a suitable user or role with access to KMS and DynamoDB. The integration tests follow [Amazon's conventions](https://docs.aws.amazon.com/sdk-for-net/latest/developer-guide/net-dg-config-creds.html) for looking up credentials. 

2. Create a key in KMS. The role created in step 1 should be able to use the key. 

3. Add the ARN for the key created in step 2 to the `KmsTestKeyArn.txt` file. This should exist on a single line without any quotes or angle brackets like:

`arn:aws:kms:eu-west-1:123456789123:key/00000000-0000-0000-0000-000000000000`

4. Run the tests