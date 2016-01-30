using System;
using System.IO;
using System.Net;
using Amazon.KeyManagementService;
using Amazon.KeyManagementService.Model;
using LeapingGorilla.SecretStore.Aws.Properties;
using LeapingGorilla.SecretStore.Interfaces;

namespace LeapingGorilla.SecretStore.Aws
{
	public class AwsKmsKeyManager : IKeyManager
	{
		private AmazonKeyManagementServiceClient CreateClient()
		{
			if (!Settings.Default.AWS_UseAccessKey)
			{
				return new AmazonKeyManagementServiceClient();
			}

			return new AmazonKeyManagementServiceClient(Settings.Default.AWS_AccessKey, Settings.Default.AWS_SecretKey);
		}

		public byte[] EncryptData(string keyId, byte[] data)
		{
			HttpStatusCode resultCode;
			int tries = 0;
			do
			{
				using (var msPlainText = new MemoryStream(data))
				using (var client = CreateClient())
				{
					var req = new EncryptRequest
					{
						KeyId = Settings.Default.AWS_KeyId,
						Plaintext = msPlainText
					};

					var result = client.Encrypt(req);
					if ((resultCode = result.HttpStatusCode) == HttpStatusCode.OK)
					{
						return result.CiphertextBlob.ToArray();
					}
				}
			} while (resultCode != HttpStatusCode.OK && ++tries <= 3);

			throw new Exception("Tried and failed 3 times to contact KMS (EncryptData)");
		}

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
						KeyId = Settings.Default.AWS_KeyId,
						KeySpec = DataKeySpec.AES_256
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
			} while (resultCode != HttpStatusCode.OK && ++tries <= 3);

			throw new Exception("Tried and failed 3 times to contact KMS (GenerateDataKey)");
		}

		public byte[] DecryptData(byte[] encryptedData)
		{
			HttpStatusCode resultCode;
			int tries = 0;
			do
			{
				using (var msData = new MemoryStream(encryptedData))
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
			} while (resultCode != HttpStatusCode.OK && ++tries <= 3);

			throw new Exception("Tried and failed 3 times to contact KMS (GenerateDataKey)");
		}
	}
}
