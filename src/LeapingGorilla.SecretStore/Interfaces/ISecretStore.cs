// /*
//    Copyright 2013-2022 Leaping Gorilla LTD
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//        http://www.apache.org/licenses/LICENSE-2.0
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// */

using System.Collections.Generic;
using System.Threading.Tasks;
using LeapingGorilla.SecretStore.Exceptions;

namespace LeapingGorilla.SecretStore.Interfaces
{
	public interface ISecretStore
	{
		/// <summary>
		/// Protects a secret and saves it to the backing secret store.
		/// </summary>
		/// <param name="keyName">Name of the key to use to protect the secret.</param>
		/// <param name="secret">The secret to save.</param>
		void ProtectAndSave(string keyName, ClearSecret secret);

		/// <summary>
		/// Gets the secret with the specified name. Returns null if no secret with the 
		/// given name exists.
		/// </summary>
		/// <param name="applicationName">The name of the application that the secret belongs to.</param>
		/// <param name="secretName">The name of the secret to retrieve.</param>
		/// <returns>The secret with the given name.</returns>
		/// <exception cref="SecretNotFoundException">Thrown if the secret cannot be found</exception>
		ClearSecret Get(string applicationName, string secretName);

		/// <summary>
		/// Gets the secret with the specified name. Returns null if no secret with the 
		/// given name exists.
		/// </summary>
		/// <param name="applicationName">The name of the application that the secret belongs to.</param>
		/// <param name="secretName">The name of the secret to retrieve.</param>
		/// <returns>The secret with the given name.</returns>
		/// <exception cref="SecretNotFoundException">Thrown if the secret cannot be found</exception>
		Task<ClearSecret> GetAsync(string applicationName, string secretName);

		/// <summary>
		/// Gets all of the secrets for a single application. Returns an empty enumeration if
		/// no secrets are found.
		/// </summary>
		/// <param name="applicationName">Name of the application to retrieve secrets for.</param>
		/// <returns>Enumeration of secrets for the application. Empty enumeration if none found</returns>
		IEnumerable<ClearSecret> GetAllForApplication(string applicationName);

		/// <summary>
		/// Asynchronously gets all of the secrets for a single application. Returns an empty enumeration if
		/// no secrets are found.
		/// </summary>
		/// <param name="applicationName">Name of the application to retrieve secrets for.</param>
		/// <returns>Enumeration of secrets for the application. Empty enumeration if none found</returns>
		Task<IEnumerable<ClearSecret>> GetAllForApplicationAsync(string applicationName);

		/// <summary>
		/// Protects the given secret, this leaves the secret in a state ready for
		/// persistence to a repository.
		/// </summary>
		/// <param name="keyName">SecretName of the key used to protect the secret.</param>
		/// <param name="secret">The secret to be protected.</param>
		/// <returns>THe secret in a protected form.</returns>
		ProtectedSecret Protect(string keyName, ClearSecret secret);

		/// <summary>
		/// Unprotects the given protected secret.
		/// </summary>
		/// <param name="protectedSecret">The protected secret.</param>
		/// <returns>An unprotected secret.</returns>
		ClearSecret Unprotect(ProtectedSecret protectedSecret);

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
