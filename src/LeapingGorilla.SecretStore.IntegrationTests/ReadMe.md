# Running Integration Tests

Integration tests by definition rely on having access to external services. For SecretStore our integration tests need to access AWS and a Postgres Database. We use files in the root of the solution to contain configuration of these items so taht we do not commit sensitive information to the repository.

# Getting Started

Tell Git that you don't want to record changes to configuration files containing potentially sensitive information.

`git update-index --assume-unchanged src/LeapingGorilla.SecretStore.IntegrationTests/KmsTestKeyArn.txt`

## Database Tests

1. Navigate to `docker/secretstore-integration-tests`

2. Start the test database using `docker compose up -d`

3. Run the tests

To clean up run `docker compose down` from the `docker/secretstore-integration-tests` directory and delete the `DockerData` directory. 

## AWS Tests

1. Create a suitable user or role with access to KMS and DynamoDB. 

2. Create a key in KMS. The role created in step 1 should be able to use the key. 

3. Add the ARN for the key created in step 2 to the `KmsTestKeyArn.txt` file. This should exist on a single line without any quotes or angle brackets like:

`arn:aws:kms:eu-west-1:123456789123:key/00000000-0000-0000-0000-000000000000`

4. Run the tests