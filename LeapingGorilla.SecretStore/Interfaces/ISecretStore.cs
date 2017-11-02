using System.Threading.Tasks;
using LeapingGorilla.SecretStore.Exceptions;

namespace LeapingGorilla.SecretStore.Interfaces
{
	public interface ISecretStore
	{
		/// <summary>
		/// Saves a secret to the backing secret store.
		/// </summary>
		/// <param name="keyName">SecretName of the key to use to protect the secret.</param>
		/// <param name="secret">The secret to save.</param>
		void Save(string keyName, Secret secret);

		/// <summary>
		/// Gets the secret with the specified name. Returns null if no secret with the 
		/// given name exists.
		/// </summary>
		/// <param name="applicationName">The name of the application that the secret belongs to.</param>
		/// <param name="secretName">The name of the secret to retrieve.</param>
		/// <returns>The secret with the given name.</returns>
		/// <exception cref="SecretNotFoundException">Thrown if the secret cannot be found</exception>
		Secret Get(string applicationName, string secretName);

		/// <summary>
		/// Gets the secret with the specified name. Returns null if no secret with the 
		/// given name exists.
		/// </summary>
		/// <param name="applicationName">The name of the application that the secret belongs to.</param>
		/// <param name="secretName">The name of the secret to retrieve.</param>
		/// <returns>The secret with the given name.</returns>
		/// <exception cref="SecretNotFoundException">Thrown if the secret cannot be found</exception>
		Task<Secret> GetAsync(string applicationName, string secretName);

		/// <summary>
		/// Protects the given secret, this leaves the secret in a state ready for
		/// persistence to a repository.
		/// </summary>
		/// <param name="keyName">SecretName of the key used to protect the secret.</param>
		/// <param name="secret">The secret to be protected.</param>
		/// <returns>THe secret in a protected form.</returns>
		ProtectedSecret Protect(string keyName, Secret secret);

		/// <summary>
		/// Unprotects the given protected secret.
		/// </summary>
		/// <param name="protectedSecret">The protected secret.</param>
		/// <returns>An unprotected secret.</returns>
		Secret Unprotect(ProtectedSecret protectedSecret);

		/// <summary>
		/// Saves the given protected secret into the secret store. The repository
		/// implementation determines how this save occurs and whether secrets are 
		/// overwritten or versioned.
		/// </summary>
		/// <param name="secret">The secret to save.</param>
		void Save(ProtectedSecret secret);

		/// <summary>
		/// Saves the given protected secret into the secret store. The repository
		/// implementation determines how this save occurs and whether secrets are 
		/// overwritten or versioned.
		/// </summary>
		/// <param name="secret">The secret to save.</param>
		Task SaveAsync(ProtectedSecret secret);
	}
}
