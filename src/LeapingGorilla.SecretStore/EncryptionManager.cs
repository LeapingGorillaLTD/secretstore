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
using System.IO;
using System.Security.Cryptography;
using LeapingGorilla.SecretStore.Interfaces;

namespace LeapingGorilla.SecretStore
{
	public class EncryptionManager : IEncryptionManager
	{
		private readonly IKeyManager _keyManager;

		public EncryptionManager(IKeyManager keyManager)
		{
			_keyManager = keyManager;
		}

		public EncryptionResult Encrypt(string keyId, byte[] dataToEncrypt)
		{
			var key = _keyManager.GenerateDataKey(keyId);
			var result = new EncryptionResult
			{
				EncryptedDataKey = key.CipherTextKey
			};

			using (var symmetricKey = CreateSymmetricKeyAlgorithm())
			{
				symmetricKey.GenerateIV();
				result.InitialisationVector = symmetricKey.IV;

				using (var encryptor = symmetricKey.CreateEncryptor(key.PlainTextKey, result.InitialisationVector))
				using (var output = new MemoryStream())
				using (var cryptoOut = new CryptoStream(output, encryptor, CryptoStreamMode.Write))
				{
					cryptoOut.Write(dataToEncrypt, 0, dataToEncrypt.Length);
					cryptoOut.FlushFinalBlock();
					result.EncryptedData = output.ToArray();
					
					cryptoOut.Clear();
				}
				symmetricKey.Clear();
			}

			return result;
		}

		public byte[] Decrypt(string keyId, byte[] encryptedDataKey, byte[] iv, byte[] encryptedData)
		{
			var key = _keyManager.DecryptData(encryptedDataKey);
			byte[] clearText;

			using (var symmetricKey = CreateSymmetricKeyAlgorithm())
			using (var decryptor = symmetricKey.CreateDecryptor(key, iv))
			using (var output = new MemoryStream())
			using (var cryptoOut = new CryptoStream(output, decryptor, CryptoStreamMode.Write))
			{
				cryptoOut.Write(encryptedData, 0, encryptedData.Length);
				cryptoOut.FlushFinalBlock();
				clearText = output.ToArray();

				cryptoOut.Clear();
				symmetricKey.Clear();
			}

			return clearText;
		}

		private SymmetricAlgorithm CreateSymmetricKeyAlgorithm()
		{
			const string algo = "AesManaged";
			var aes = Aes.Create(algo) 
				?? throw new ApplicationException($"Failed to create SymmetricAlgorithm - {algo}");
			aes.Mode = CipherMode.CBC;
			aes.Padding = PaddingMode.PKCS7;

			return aes;
		}
	}
}