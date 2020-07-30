namespace LeapingGorilla.SecretStore.Exceptions
{
	///<summary>Thrown if the data passed to an encryption manager does not appear to be in the expected format</summary>
	public class InvalidDataFormatException : SecretStoreException
	{
		public InvalidDataFormatException() : base("The data that you provided does not appear to be in the expected format") { }
	}
}
