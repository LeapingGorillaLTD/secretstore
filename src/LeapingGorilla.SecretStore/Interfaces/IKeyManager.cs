﻿// /*
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

using System;
using LeapingGorilla.SecretStore.Exceptions;

namespace LeapingGorilla.SecretStore.Interfaces
{
	///<summary>Global key manager used to encrypt or decrypt per-document keys</summary>
	public interface IKeyManager
	{
		/// <summary>
		/// Encrypts the passed data with a Master Key.
		/// </summary>
		/// <param name="keyId">The identifier for the master key.</param>
		/// <param name="data">The data to encrypt.</param>
		/// <returns>Encrypted data.</returns>
		/// <exception cref="ArgumentNullException">Data is null</exception>
		/// <exception cref="ArgumentException">Data is empty</exception>
		/// <exception cref="PayloadTooLargeException">Data is too large to be encrypted</exception>
		/// <exception cref="MasterKeyNotFoundException">No key could be found matching <see cref="keyId"/></exception>
		byte[] EncryptData(string keyId, byte[] data);

		/// <summary>
		/// Generates a data key for the specified master key. This will return both
		/// the plaintext key that you can use for encryption/decryption and the 
		/// same key as protected by the master key.
		/// </summary>
		/// <param name="keyId">
		/// The identifier of the key we should use to generate and protect the data key.
		/// </param>
		/// <returns>
		/// Result containing the plaintext representation of the generated data key
		/// which can be used for encryption/decryption, an encrypted version of the 
		/// data key encrypted by the master key and the ID of the key that was used.
		/// </returns>
		GenerateDataKeyResult GenerateDataKey(string keyId);

		/// <summary>
		/// Decrypts the passed data with a master key.
		/// </summary>
		/// <param name="data">The encrypted data to decrypt.</param>
		/// <returns>Resulting clear text.</returns>
		/// <exception cref="ArgumentNullException">Data is null</exception>
		/// <exception cref="ArgumentException">Data is empty</exception>
		/// <exception cref="PayloadTooLargeException">Data is too large to be decrypted</exception>
		/// <exception cref="MasterKeyNotFoundException">No key could be found matching <see cref="keyId"/></exception>
		byte[] DecryptData(byte[] data);
	}
}
