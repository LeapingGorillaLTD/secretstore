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

			using (var symmetricKey = CreateSymmetricKey())
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

			using (var symmetricKey = CreateSymmetricKey())
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

		private AesManaged CreateSymmetricKey()
		{
			return new AesManaged
			{
				Mode = CipherMode.CBC,
				Padding = PaddingMode.PKCS7
			};
		}
	}
}
