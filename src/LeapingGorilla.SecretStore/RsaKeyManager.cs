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
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using LeapingGorilla.SecretStore.Exceptions;
using LeapingGorilla.SecretStore.Interfaces;

namespace LeapingGorilla.SecretStore
{
	public class RsaKeyManager : IRsaKeyManager
	{
		public const int DataKeyLengthInBytes = 128 / 8; // 128-bit key, 8 bits per byte == 16 bytes

		private static readonly HashAlgorithmName SigningAlgorithm = HashAlgorithmName.SHA512;
		private static readonly RSASignaturePadding SigningPadding = RSASignaturePadding.Pkcs1;

		private readonly IRsaKeyStore _keyStore;

		public RsaKeyManager(IRsaKeyStore keyStore)
		{
			_keyStore = keyStore;
		}

		public byte[] EncryptData(string keyId, byte[] data)
		{
			var key = _keyStore.GetPublicKey(keyId);

			if (data == null)
			{
				throw new ArgumentNullException(nameof(data));
			}

			if (!data.Any())
			{
				throw new ArgumentException("You must provide some data to encrypt", nameof(data));
			}

			var maxPayload = MaxEncryptionPayloadSizeInBytes(key.Modulus.Length);
			if (data.Length > maxPayload)
			{
				throw new PayloadTooLargeException(maxPayload, data.Length);
			}


			byte[] encrypted;
			using (var rsa = new RSACryptoServiceProvider())
			{
				rsa.ImportParameters(key);
				encrypted =  rsa.Encrypt(data, true);
			}

			return PrependKeyIdSegment(keyId, encrypted);
		}

		public GenerateDataKeyResult GenerateDataKey(string keyId)
		{
			if (keyId == null)
			{
				throw new ArgumentNullException(nameof(keyId));
			}

			if (String.IsNullOrWhiteSpace(keyId))
			{
				throw new ArgumentException("You must provide a key ID", nameof(keyId));
			}
			
			var result = new GenerateDataKeyResult
			{
				KeyId = keyId,
				PlainTextKey = new byte[DataKeyLengthInBytes]
			};

			var plainKey = new byte[DataKeyLengthInBytes];
			RandomNumberGenerator.Fill(plainKey);
			Array.Copy(plainKey, result.PlainTextKey, DataKeyLengthInBytes);

			result.CipherTextKey = EncryptData(keyId, plainKey);
			Array.Clear(plainKey, 0, plainKey.Length);

			return result;
		}

		public byte[] DecryptData(byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException(nameof(data));
			}

			if (!data.Any())
			{
				throw new ArgumentException("You must provide some data to encrypt", nameof(data));
			}

			var key = _keyStore.GetPrivateKey(GetKeyId(data, out var keyIdSegmentLength));
			var toDecrypt = new byte[data.Length - keyIdSegmentLength];
			Buffer.BlockCopy(data, keyIdSegmentLength, toDecrypt, 0, toDecrypt.Length);
			
			using (var rsa = new RSACryptoServiceProvider())
			{
				rsa.ImportParameters(key);
				return rsa.Decrypt(toDecrypt, true);
			}
		}

		private int MaxEncryptionPayloadSizeInBytes(int modulusSizeInBytes)
		{
			const int hashSizeInBytes = 160 / 8; // Default hash is SHA-1, size is 160-bits == 20 bytes

			return (int)Math.Floor(modulusSizeInBytes - 2m - (2 * hashSizeInBytes));
		}

		/// <summary>
		/// Generates a key name segment and prepends it to the passed protected data.
		/// </summary>
		/// <param name="keyId">The key identifier we are generating a segment for.</param>
		/// <param name="protectedData">The protected data.</param>
		/// <returns>Byte array containing a key ID and the protected data.</returns>
		public byte[] PrependKeyIdSegment(string keyId, byte[] protectedData)
		{
			var keyIdBytes = Encoding.UTF8.GetBytes(keyId);

			var toSign = new byte[keyIdBytes.Length + protectedData.Length];
			Buffer.BlockCopy(keyIdBytes, 0, toSign, 0, keyIdBytes.Length);
			Buffer.BlockCopy(protectedData, 0, toSign, keyIdBytes.Length, protectedData.Length);

			byte[] sig;

			using (var signingKey = new RSACryptoServiceProvider())
			{
				signingKey.ImportParameters(_keyStore.GetSigningKey());
				sig = signingKey.SignData(toSign, SigningAlgorithm, SigningPadding);
			}

			byte[] keyNameSegment = new byte[8 + keyIdBytes.Length + sig.Length + protectedData.Length];
			Buffer.BlockCopy(BitConverter.GetBytes(keyIdBytes.Length), 0, keyNameSegment, 0, 4);
			Buffer.BlockCopy(BitConverter.GetBytes(sig.Length), 0, keyNameSegment, 4, 4);
			Buffer.BlockCopy(sig, 0, keyNameSegment, 8, sig.Length);
			Buffer.BlockCopy(toSign, 0, keyNameSegment, 8 + sig.Length, toSign.Length);

			return keyNameSegment;
		}

		public string GetKeyId(byte[] protectedData, out int keyIdSegmentLength)
		{
			const int idxEndOfLengths = 8; // 2 x Int32 at start of data block account for 8 bytes

			if (protectedData == null || !protectedData.Any() || protectedData.Length < idxEndOfLengths)
			{
				throw new InvalidDataFormatException();
			}

			var keyNameSize = BitConverter.ToInt32(protectedData, 0);
			var sigSize = BitConverter.ToInt32(protectedData, 4);

			keyIdSegmentLength = idxEndOfLengths + keyNameSize + sigSize;

			if (protectedData.Length < keyIdSegmentLength)
			{
				throw new InvalidDataFormatException();
			}
			
			var sig = new byte[sigSize];
			Buffer.BlockCopy(protectedData, idxEndOfLengths, sig, 0, sigSize);

			var sizeOfProtectedDataPlusKeyIdBytes = protectedData.Length - idxEndOfLengths - sigSize;
			
			using (var signingKey = new RSACryptoServiceProvider())
			{
				signingKey.ImportParameters(_keyStore.GetSigningKey());

				if (!signingKey.VerifyData(protectedData, idxEndOfLengths + sigSize, sizeOfProtectedDataPlusKeyIdBytes, sig, SigningAlgorithm, SigningPadding))
				{
					throw new InvalidDataFormatException();
				}
			}

			return Encoding.UTF8.GetString(protectedData, idxEndOfLengths + sigSize, keyNameSize);
		}
	}
}