The `AwsDynamoProtectedSecretRepository` is a [SecretRepository](SecretRepository) which is backed by an AWS Dynamo Table. 

# Dynamo

## Required Permissions ##

Your secret store code requires the following [AWS DynamoDB permissions](http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/api-permissions-reference.html) to be available at run time.

Permission    | Reason
--------------|---------
DescribeTable | Get the table schema on initialisation
GetItem       | Retrieve a single [ProtectedSecret](ProtectedSecret)
PutItem       | Store a single [ProtectedSecret](ProtectedSecret)
CreateTable   | *(Optional)* Only required if you wish to create a table in code at run time

## Schema ##

The table that the `AwsDynamoProtectedSecretRepository` is backed by has the following schema.

** The schema definition is case sensitive **

Attribute Name       | Type              | Purpose
---------------------|-------------------|---------
SecretName                 | String (Partition Key) | The name of the secret
MasterKeyId                | String            | ID of the key used to protect the document key. By convention this is the ARN of the KMS key that you will use to protect the key (like `arn:aws:kms:eu-west-1:123456123456:key/abcd1234-abcd-1234-abcd-1234abcd1234`). You may use an alias for this which will require you to use the format **alias/**YourKeyAlias as the KeyId and provide the [DescribeKey KMS permission](https://docs.aws.amazon.com/kms/latest/developerguide/kms-api-permissions-reference.html).
ProtectedDocumentKey         | Binary            | Document Key used to protect the secret value. This key is encrypted with the key identified by KeyId
ProtectedSecretValue       | Binary            | Value of the secret. This is encrypted with the document key
InitialisationVector | Binary            | IV used to encrypt the secret value with the document key. This is not a secret.

This class is often used alongside the [AwsKmsKeyManager](AwsKmsKeyManager)