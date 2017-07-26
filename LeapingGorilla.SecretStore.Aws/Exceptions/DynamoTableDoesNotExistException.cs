using LeapingGorilla.SecretStore.Exceptions;

namespace LeapingGorilla.SecretStore.Aws.Exceptions
{
	public class DynamoTableDoesNotExistException : SecretStoreException
	{
		public DynamoTableDoesNotExistException(string tableName) : base($"The Dynamo Table {tableName} could not be loaded. Check that the table exists and that the executing code has read & write priviliges")
		{
		}
	}
}
