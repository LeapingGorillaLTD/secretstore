using System;
using System.IO;
using System.Linq;
using System.Net;
using Amazon.KeyManagementService;
using Amazon.KeyManagementService.Model;
using LeapingGorilla.SecretStore.Aws.Exceptions;
using LeapingGorilla.SecretStore.Exceptions;
using LeapingGorilla.SecretStore.Interfaces;

namespace LeapingGorilla.SecretStore.Aws
{
	public class AwsKmsKeyManager : IKeyManager
	{
		public const int MaxEncryptPayloadSize = 4096;
		public const int MaxDecryptPayloadSize = 6144;

		private AmazonKeyManagementServiceClient CreateClient()
		{
			return new AmazonKeyManagementServiceClient();
		}

		/// <summary>
		/// Encrypts the passed data with a Master Key. This method will encrypt
		/// a payload up to a maximum size of 4096 bytes. The master key with the
		/// specified ID will be used to encrypt the data.
		/// </summary>
		/// <param name="keyId">The identifier for the master key.</param>
		/// <param name="data">The data to encrypt.</param>
		/// <returns>Encrypted data.</returns>
		/// <exception cref="System.ArgumentNullException"></exception>
		/// <exception cref="System.ArgumentException">You must provide some data to encrypt</exception>
		/// <exception cref="PayloadTooLargeException"></exception>
		/// <exception cref="KeyManagementServiceUnavailable"></exception>
		public byte[] EncryptData(string keyId, byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException(nameof(data));
			}

			if (!data.Any())
			{
				throw new ArgumentException("You must provide some data to encrypt", nameof(data));
			}

			if (data.Length > MaxEncryptPayloadSize)
			{
				throw new PayloadTooLargeException(MaxEncryptPayloadSize, data.Length);
			}

			HttpStatusCode resultCode;
			int tries = 0;
			do
			{
				using (var msPlainText = new MemoryStream(data))
				using (var client = CreateClient())
				{
					var req = new EncryptRequest
					{
						KeyId = keyId,
						Plaintext = msPlainText
					};

					var result = client.Encrypt(req);
					if ((resultCode = result.HttpStatusCode) == HttpStatusCode.OK)
					{
						return result.CiphertextBlob.ToArray();
					}
				}
			} while (ShouldRetry(++tries, resultCode));

			throw new KeyManagementServiceUnavailable(resultCode);
		}

		/// <summary>
		/// Generates a data key for the specified master key. This will return both
		/// the plaintext key that you can use for encryption/decryption and the
		/// same key as protected by the master key.
		/// </summary>
		/// <param name="keyId">The identifier of the key we should use to generate and protect the data key.</param>
		/// <returns>Result containing the plaintext representation of the generated data key
		/// which can be used for encryption/decryption, an encrypted version of the
		/// data key encrypted by the master key and the ID of the key that was used.</returns>
		/// <exception cref="KeyManagementServiceUnavailable"></exception>
		public GenerateDataKeyResult GenerateDataKey(string keyId)
		{
			HttpStatusCode resultCode;
			int tries = 0;
			do
			{
				using (var client = CreateClient())
				{
					var req = new GenerateDataKeyRequest
					{
						KeyId = keyId,
						KeySpec = DataKeySpec.AES_128
					};

					var result = client.GenerateDataKey(req);
					if ((resultCode = result.HttpStatusCode) == HttpStatusCode.OK)
					{
						return new GenerateDataKeyResult
						{
							KeyId = keyId,
							CipherTextKey = result.CiphertextBlob.ToArray(),
							PlainTextKey = result.Plaintext.ToArray()
						};
					}
				}
			} while (ShouldRetry(++tries, resultCode));

			throw new KeyManagementServiceUnavailable(resultCode);
		}

		/// <summary>
		/// Decrypts the passed data with a master key. This method will decrypt a
		/// payload of up to 6144 bytes returning the resulting clear text
		/// </summary>
		/// <param name="keyId">The id of the key used to unprotect the data.</param>
		/// <param name="data">The encrypted data to decrypt.</param>
		/// <returns>Resulting clear text.</returns>
		/// <exception cref="System.ArgumentNullException"></exception>
		/// <exception cref="System.ArgumentException">You must provide some data to encrypt</exception>
		/// <exception cref="PayloadTooLargeException"></exception>
		/// <exception cref="KeyManagementServiceUnavailable"></exception>
		public byte[] DecryptData(string keyId, byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException(nameof(data));
			}

			if (!data.Any())
			{
				throw new ArgumentException("You must provide some data to encrypt", nameof(data));
			}

			if (data.Length > MaxDecryptPayloadSize)
			{
				throw new PayloadTooLargeException(MaxDecryptPayloadSize, data.Length);
			}

			HttpStatusCode resultCode;
			int tries = 0;
			do
			{
				using (var msData = new MemoryStream(data))
				using (var client = CreateClient())
				{
					var req = new DecryptRequest
					{
						CiphertextBlob = msData
					};

					var result = client.Decrypt(req);
					if ((resultCode = result.HttpStatusCode) == HttpStatusCode.OK)
					{
						return result.Plaintext.ToArray();
					}
				}
			} while (ShouldRetry(++tries, resultCode));

			throw new KeyManagementServiceUnavailable(resultCode);
		}

		private static bool ShouldRetry(int counter, HttpStatusCode statusCode)
		{
			bool codeCanBeRetried;
			switch ((int)statusCode)
			{
				case 500:
				case 503:
					codeCanBeRetried = true;
					break;

				default:
					codeCanBeRetried = false;
					break;
			}

			return codeCanBeRetried && counter < 3;
		}
	}
}
