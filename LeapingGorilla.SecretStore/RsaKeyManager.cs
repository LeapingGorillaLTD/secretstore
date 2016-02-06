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
		private static readonly RSASignaturePadding SigningPadding = RSASignaturePadding.Pss;

		private readonly IRsaKeyStore _keyStore;

		private static readonly RNGCryptoServiceProvider RandomNumberGenerator = new RNGCryptoServiceProvider();

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

			byte[] plainKey = new byte[DataKeyLengthInBytes];
			RandomNumberGenerator.GetBytes(plainKey);
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

			int keyIdSegmentLength;
			var key = _keyStore.GetPrivateKey(GetKeyId(data, out keyIdSegmentLength));
			var toDecrypt = new byte[data.Length - keyIdSegmentLength];
			Buffer.BlockCopy(data, keyIdSegmentLength, toDecrypt, 0, toDecrypt.Length);

			var maxPayload = key.Modulus.Length; // Size of the key in bytes
			if (toDecrypt.Length > maxPayload)
			{
				throw new PayloadTooLargeException(maxPayload, toDecrypt.Length);
			}
			
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

			byte[] keyNameSegment = new byte[8 + keyIdBytes.Length + sig.Length];
			Buffer.BlockCopy(BitConverter.GetBytes(keyIdBytes.Length), 0, keyNameSegment, 0, 4);
			Buffer.BlockCopy(BitConverter.GetBytes(sig.Length), 0, keyNameSegment, 4, 4);
			Buffer.BlockCopy(keyIdBytes, 0, keyNameSegment, 8, keyIdBytes.Length);
			Buffer.BlockCopy(sig, 0, keyNameSegment, 8 + keyIdBytes.Length, sig.Length);

			return keyNameSegment;
		}

		public string GetKeyId(byte[] protectedData, out int keyIdSegmentLength)
		{
			if (protectedData.Length < 8)
			{
				throw new InvalidDataFormatException();
			}

			var keyNameSize = BitConverter.ToInt32(protectedData, 0);
			var sigSize = BitConverter.ToInt32(protectedData, 4);

			keyIdSegmentLength = 8 + keyNameSize + sigSize;

			if (protectedData.Length < keyNameSize + sigSize + 8)
			{
				throw new InvalidDataFormatException();
			}
			
			var sig = new byte[sigSize];
			Buffer.BlockCopy(protectedData, 8 + keyNameSize, sig, 0, sigSize);

			var toSign = new byte[protectedData.Length - keyIdSegmentLength];
			Buffer.BlockCopy(protectedData, 8, toSign, 0, keyNameSize);
			Buffer.BlockCopy(protectedData, keyIdSegmentLength, toSign, keyNameSize, protectedData.Length - keyIdSegmentLength);

			using (var signingKey = new RSACryptoServiceProvider())
			{
				signingKey.ImportParameters(_keyStore.GetSigningKey());

				if (!signingKey.VerifyData(toSign, sig, SigningAlgorithm, SigningPadding))
				{
					throw new InvalidDataFormatException();
				}
			}

			return Encoding.UTF8.GetString(protectedData, 8, keyNameSize);
		}
	}
}
