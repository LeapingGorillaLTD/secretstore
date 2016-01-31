namespace LeapingGorilla.SecretStore.Interfaces
{
	public interface IProtectedSecretRepository
	{
		ProtectedSecret Get(string name);

		void Save(ProtectedSecret secret);
	}
}
