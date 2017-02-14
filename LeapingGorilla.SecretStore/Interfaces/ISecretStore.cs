namespace LeapingGorilla.SecretStore.Interfaces
{
	public interface ISecretStore
	{
		/// <summary>
		/// Saves a secret to the backing secret store.
		/// </summary>
		/// <param name="keyName">Name of the key to use to protect the secret.</param>
		/// <param name="secret">The secret to save.</param>
		void Save(string keyName, Secret secret);

		/// <summary>
		/// Gets the secret with the specified name. Returns null if no secret with the 
		/// given name exists.
		/// </summary>
		/// <param name="name">The name of the secret to retrieve.</param>
		/// <returns>The secret with the given name.</returns>
		Secret Get(string name);

		/// <summary>
		/// Protects the given secret, this leaves the secret in a state ready for
		/// persistence to a repository.
		/// </summary>
		/// <param name="keyName">Name of the key used to protect the secret.</param>
		/// <param name="secret">The secret to be protected.</param>
		/// <returns>THe secret in a protected form.</returns>
		ProtectedSecret Protect(string keyName, Secret secret);

		/// <summary>
		/// Unprotects the given protected secret.
		/// </summary>
		/// <param name="protectedSecret">The protected secret.</param>
		/// <returns>An unprotected secret.</returns>
		Secret Unprotect(ProtectedSecret protectedSecret);
	}
}
