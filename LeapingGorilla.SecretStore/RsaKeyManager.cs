using System;
using System.Linq;
using System.Security.Cryptography;
using LeapingGorilla.SecretStore.Exceptions;
using LeapingGorilla.SecretStore.Interfaces;

namespace LeapingGorilla.SecretStore
{
	public class RsaKeyManager : IKeyManager
	{
		public const int DataKeyLengthInBytes = 128 / 8; // 128-bit key, 8 bits per byte == 16 bytes

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

			
			using (var rsa = new RSACryptoServiceProvider())
			{
				rsa.ImportParameters(key);
				return rsa.Encrypt(data, true);
			}
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

		public byte[] DecryptData(string keyId, byte[] data)
		{
			var key = _keyStore.GetPrivateKey(keyId);

			if (data == null)
			{
				throw new ArgumentNullException(nameof(data));
			}

			if (!data.Any())
			{
				throw new ArgumentException("You must provide some data to encrypt", nameof(data));
			}

			var maxPayload = key.Modulus.Length; // Size of the key in bytes
			if (data.Length > maxPayload)
			{
				throw new PayloadTooLargeException(maxPayload, data.Length);
			}
			
			using (var rsa = new RSACryptoServiceProvider())
			{
				rsa.ImportParameters(key);
				return rsa.Decrypt(data, true);
			}
		}

		private int MaxEncryptionPayloadSizeInBytes(int modulusSizeInBytes)
		{
			const int hashSizeInBytes = 160 / 8; // Default hash is SHA-1, size is 160-bits == 20 bytes

			return (int)Math.Floor(modulusSizeInBytes - 2m - (2 * hashSizeInBytes));
		}

	}
}
