using System.Threading.Tasks;

namespace LeapingGorilla.SecretStore.Interfaces
{
	public interface IProtectedSecretRepository
	{
		ProtectedSecret Get(string applicationName, string secretName);

		Task<ProtectedSecret> GetAsync(string applicationName, string secretName);

		void Save(ProtectedSecret secret);

		Task SaveAsync(ProtectedSecret secret);
	}
}
