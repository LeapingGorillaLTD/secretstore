using Amazon.DynamoDBv2.DocumentModel;

namespace LeapingGorilla.SecretStore.Aws
{
	public static class AwsExtensions
	{
		public static ProtectedSecret ToProtectedSecret(this Document document)
		{
			return new ProtectedSecret
			{
				ApplicationName = document[AwsDynamoProtectedSecretRepository.Fields.ApplicationName].AsString(),
				Name = document[AwsDynamoProtectedSecretRepository.Fields.SecretName].AsString(),
				MasterKeyId = document[AwsDynamoProtectedSecretRepository.Fields.MasterKeyId].AsString(),
				ProtectedDocumentKey = document[AwsDynamoProtectedSecretRepository.Fields.ProtectedDocumentKey].AsByteArray(),
				ProtectedSecretValue = document[AwsDynamoProtectedSecretRepository.Fields.ProtectedSecretValue].AsByteArray(),
				InitialisationVector = document[AwsDynamoProtectedSecretRepository.Fields.InitialisationVector].AsByteArray()
			};
		}
	}
}
