# KMS

## Permissions ##

For each master key that you want to use with the system you should allow the [following KMS permissions](https://docs.aws.amazon.com/kms/latest/developerguide/kms-api-permissions-reference.html):

Permission        | Reason
------------------|---------
GenerateDataKey   | Generate the Document Key that will be used to protect the secret
Decrypt           | Allows the document key to be decrypted so we can then decrypt the secret
DescribeKey       | *(Optional)* Allows a master key to be looked up by an alias rather than using a full ARN. If using an alias the key id should be in the format **alias/**yourkeyname


This key manager is often used alongside the [AwsDynamoProtectedSecretRepository](AwsDynamoProtectedSecretRepository)