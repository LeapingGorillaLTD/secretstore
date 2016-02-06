namespace LeapingGorilla.SecretStore
{
	public class ProtectedSecret
	{
		public string Name { get; set; }

		public string KeyId { get; set; }

		public byte[] ProtectedValue { get; set; }

		public byte[] ProtectedKey { get; set; }

		public byte[] InitialisationVector { get; set; }
	}
}
