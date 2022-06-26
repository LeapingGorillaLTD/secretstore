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

using System.Security.Cryptography;

namespace LeapingGorilla.SecretStore.Interfaces
{
	///<summary>
	/// Retrieves an RSA Public or Private key for use by the Key Manager.
	/// Note that the RSA key should have a length of at least 4096 bits so
	/// that we can support the maximum constraint on the Key Manager 
	/// interface
	/// </summary>
	public interface IRsaKeyStore
	{
		/// <summary>
		/// Gets the public key for the key with the given ID.
		/// </summary>
		/// <param name="keyId">The key identifier.</param>
		/// <returns>RSAParameters for the public key with the given name.</returns>
		RSAParameters GetPublicKey(string keyId);

		/// <summary>
		/// Gets the private key for the key with the specified ID.
		/// </summary>
		/// <param name="keyId">The key identifier.</param>
		/// <returns>RSAParameters for the private key with the given name.</returns>
		RSAParameters GetPrivateKey(string keyId);

		/// <summary>
		/// Gets the key which is used for signing requests. This should be a 
		/// separate key which is always available.
		/// </summary>
		/// <returns>RSAParameters for a signing key.</returns>
		RSAParameters GetSigningKey();
	}
}
