using System.Collections.Generic;
using System.Threading.Tasks;
using LeapingGorilla.SecretStore.Exceptions;

namespace LeapingGorilla.SecretStore.Interfaces
{
	public interface IProtectedSecretRepository
	{
		/// <summary>
		/// Retrieve a protected secret from the backing data store
		/// </summary>
		/// <param name="applicationName">Name of the application of the secret</param>
		/// <param name="secretName">Name of the secret to retrieve</param>
		/// <returns>
		/// A <see cref="ProtectedSecret"/> for the given Application Name/Secret Name pair.
		/// A <see cref="SecretNotFoundException"/> will be thrown if none found.
		/// </returns>
		/// <exception cref="SecretNotFoundException">
		/// Thrown if no secret found for the give application/secret name combination
		/// </exception>
		ProtectedSecret Get(string applicationName, string secretName);

		/// <summary>
		/// Retrieve a protected secret from the backing data store asynchronously
		/// </summary>
		/// <param name="applicationName">Name of the application of the secret</param>
		/// <param name="secretName">Name of the secret to retrieve</param>
		/// <returns>
		/// A <see cref="ProtectedSecret"/> for the given Application Name/Secret Name pair.
		/// A <see cref="SecretNotFoundException"/> will be thrown if none found.
		/// </returns>
		/// <exception cref="SecretNotFoundException">
		/// Thrown if no secret found for the give application/secret name combination
		/// </exception>
		Task<ProtectedSecret> GetAsync(string applicationName, string secretName);

		/// <summary>
		/// Save the given <see cref="ProtectedSecret"/> to the backing data store.
		/// </summary>
		/// <param name="secret">The <see cref="ProtectedSecret"/> to be saved</param>
		void Save(ProtectedSecret secret);

		/// <summary>
		/// Save the given <see cref="ProtectedSecret"/> to the backing data store asynchronously.
		/// </summary>
		/// <param name="secret">The <see cref="ProtectedSecret"/> to be saved</param>
		Task SaveAsync(ProtectedSecret secret);

		/// <summary>
		/// Gets all secrets for a single application.
		/// </summary>
		/// <param name="applicationName">Name of the application to retrieve secrets for.</param>
		/// <returns>Enumeration of all of the secrets for a single application</returns>
		IEnumerable<ProtectedSecret> GetAllForApplication(string applicationName);

		/// <summary>
		/// Gets all secrets for a single application iasynchronously.
		/// </summary>
		/// <param name="applicationName">Name of the application to retrieve secrets for.</param>
		/// <returns>Enumeration of all of the secrets for a single application</returns>
		Task<IEnumerable<ProtectedSecret>> GetAllForApplicationAsync(string applicationName);
	}
}