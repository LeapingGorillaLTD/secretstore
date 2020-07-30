using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeapingGorilla.SecretStore.Interfaces
{
	public interface IProtectedSecretRepository
	{
		ProtectedSecret Get(string applicationName, string secretName);

		Task<ProtectedSecret> GetAsync(string applicationName, string secretName);

		void Save(ProtectedSecret secret);

		Task SaveAsync(ProtectedSecret secret);

		/// <summary>
		/// Gets all secrets for a single application.
		/// </summary>
		/// <param name="applicationName">Name of the application to retrieve secrets for.</param>
		/// <returns>Enumeration of all of the secrets for a single application</returns>
		IEnumerable<ProtectedSecret> GetAllForApplication(string applicationName);

		/// <summary>
		/// Gets all secrets for a single application in an async manner.
		/// </summary>
		/// <param name="applicationName">Name of the application to retrieve secrets for.</param>
		/// <returns>Enumeration of all of the secrets for a single application</returns>
		Task<IEnumerable<ProtectedSecret>> GetAllForApplicationAsync(string applicationName);
	}
}
