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

using System;
using System.Text;
using LeapingGorilla.SecretStore.Interfaces;

namespace LeapingGorilla.SecretStore
{
	///<summary>Provides common shared variables (like the encoding to use) for secret stores</summary>
	public abstract class BaseSecretStore
	{
		protected readonly IProtectedSecretRepository _secrets;
		protected readonly IEncryptionManager _encryptionManager;

		public static readonly Encoding SecretEncoding = Encoding.UTF8;

		protected BaseSecretStore(
			IProtectedSecretRepository secrets, 
			IEncryptionManager encryptionManager)
		{
			_secrets = secrets;
			_encryptionManager = encryptionManager;
		}

		public virtual ProtectedSecret Protect(string keyName, ClearSecret secret)
		{
			if (secret == null)
			{
				throw new ArgumentNullException(nameof(secret));
			}

			var encryptedSecret = _encryptionManager.Encrypt(keyName, SecretEncoding.GetBytes(secret.Value));
			return new ProtectedSecret
			{
				ApplicationName = secret.ApplicationName,
				MasterKeyId = keyName,
				InitialisationVector = encryptedSecret.InitialisationVector,
				Name = secret.Name,
				ProtectedDocumentKey = encryptedSecret.EncryptedDataKey,
				ProtectedSecretValue = encryptedSecret.EncryptedData
			};
		}

		public virtual ClearSecret Unprotect(ProtectedSecret protectedSecret)
		{
			if (protectedSecret == null)
			{
				throw new ArgumentNullException(nameof(protectedSecret));
			}

			var rawValue = _encryptionManager.Decrypt(protectedSecret.MasterKeyId, protectedSecret.ProtectedDocumentKey, protectedSecret.InitialisationVector, protectedSecret.ProtectedSecretValue);

			return new ClearSecret(protectedSecret.ApplicationName, protectedSecret.Name, SecretEncoding.GetString(rawValue));
		}
	}
}
