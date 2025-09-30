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
using System.Threading;
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
		/// <param name="cancellationToken">Optional cancellation token</param>
		/// <returns>
		/// A <see cref="ProtectedSecret"/> for the given Application Name/Secret Name pair.
		/// A <see cref="SecretNotFoundException"/> will be thrown if none found.
		/// </returns>
		/// <exception cref="SecretNotFoundException">
		/// Thrown if no secret found for the give application/secret name combination
		/// </exception>
		Task<ProtectedSecret> GetAsync(string applicationName, string secretName, CancellationToken cancellationToken = default);

		/// <summary>
		/// Save the given <see cref="ProtectedSecret"/> to the backing data store.
		/// </summary>
		/// <param name="secret">The <see cref="ProtectedSecret"/> to be saved</param>
		void Save(ProtectedSecret secret);

		/// <summary>
		/// Save the given <see cref="ProtectedSecret"/> to the backing data store asynchronously.
		/// </summary>
		/// <param name="secret">The <see cref="ProtectedSecret"/> to be saved</param>
		/// <param name="cancellationToken">Optional cancellation token</param>
		Task SaveAsync(ProtectedSecret secret, CancellationToken cancellationToken = default);

		/// <summary>
		/// Gets all secrets for a single application.
		/// </summary>
		/// <param name="applicationName">Name of the application to retrieve secrets for.</param>
		/// <returns>Enumeration of all secrets for a single application</returns>
		IEnumerable<ProtectedSecret> GetAllForApplication(string applicationName);

		/// <summary>
		/// Gets all secrets for a single application asynchronously.
		/// </summary>
		/// <param name="applicationName">Name of the application to retrieve secrets for.</param>
		/// <param name="cancellationToken">Optional cancellation token</param>
		/// <returns>Enumeration of all secrets for a single application</returns>
		Task<IEnumerable<ProtectedSecret>> GetAllForApplicationAsync(string applicationName, CancellationToken cancellationToken = default);
	}
}