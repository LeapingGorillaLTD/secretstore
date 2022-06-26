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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using Amazon;
using Amazon.KeyManagementService;
using Amazon.KeyManagementService.Model;
using Amazon.Runtime;
using LeapingGorilla.SecretStore.Aws.Exceptions;
using LeapingGorilla.SecretStore.Exceptions;
using LeapingGorilla.SecretStore.Interfaces;
using Polly;

namespace LeapingGorilla.SecretStore.Aws
{
	public class AwsKmsKeyManager : IKeyManager, IDisposable
	{
		public const int MaxEncryptPayloadSize = 4096;
		public const int MaxDecryptPayloadSize = 6144;

		private IAmazonKeyManagementService _client;

		private bool _disposed;

		private static readonly Policy<AmazonWebServiceResponse> RetryPolicy =
			Policy
				.Handle<KMSInternalException>()
				.Or<KeyUnavailableException>()
				.Or<DependencyTimeoutException>()
				.OrResult<AmazonWebServiceResponse>(res => (int)res.HttpStatusCode == 503 || (int)res.HttpStatusCode == 500)
				.Retry(2);


		/// <summary>
		/// Initializes a new instance of the <see cref="AwsKmsKeyManager"/> class.
		/// </summary>
		/// <param name="regionSystemName">Name of the region system i.e. eu-west-1 or us-west-2.</param>
		/// <exception cref="System.ArgumentException">You must provide a Region System Name (i.e. eu-west-1 or us-west-2; see http://docs.aws.amazon.com/general/latest/gr/rande.html for the full list)</exception>
		public AwsKmsKeyManager(string regionSystemName)
		{
			if (String.IsNullOrWhiteSpace(regionSystemName))
			{
				throw new ArgumentException("You must provide a Region System Name (i.e. eu-west-1 or us-west-2; see http://docs.aws.amazon.com/general/latest/gr/rande.html for the full list)");
			}

			var regionEndpoint = RegionEndpoint.GetBySystemName(regionSystemName.ToLowerInvariant());
			_client = new AmazonKeyManagementServiceClient(regionEndpoint);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AwsKmsKeyManager"/> class.
		/// </summary>
		/// <param name="client">The KMS client instance to use.</param>
		public AwsKmsKeyManager(IAmazonKeyManagementService client)
		{
			_client = client;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AwsKmsKeyManager"/> class.
		/// </summary>
		/// <param name="endpoint">The region endpoint i.e. eu-west-1 or us-west-2.</param>
		/// <exception cref="ArgumentException">You must provide a Region System Name (i.e. eu-west-1 or us-west-2; see http://docs.aws.amazon.com/general/latest/gr/rande.html for the full list)</exception>
		[ExcludeFromCodeCoverage]
		public AwsKmsKeyManager(RegionEndpoint endpoint)
		{
			var regionEndpoint = endpoint ?? throw new ArgumentException("You must provide a Region System Name (i.e. eu-west-1 or us-west-2; see http://docs.aws.amazon.com/general/latest/gr/rande.html for the full list)");
			_client = new AmazonKeyManagementServiceClient(regionEndpoint);
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
		/// <exception cref="KeyManagementServiceUnavailableException"></exception>
		public byte[] EncryptData(string keyId, byte[] data)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException("AwsKmsKeyManager");
			}

			ValidateMasterKey(keyId);

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
			
			var res = RetryPolicy.ExecuteAndCapture(() =>
			{
				using (var msPlainText = new MemoryStream(data))
				{
					var req = new EncryptRequest
					{
						KeyId = keyId,
						Plaintext = msPlainText
					};

					var t = _client.EncryptAsync(req);
					t.ConfigureAwait(false);
					return t.GetAwaiter().GetResult();
				}
			});

			ValidateResponse(res);

			return ((EncryptResponse)res.Result).CiphertextBlob.ToArray();
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
		/// <exception cref="KeyManagementServiceUnavailableException"></exception>
		public GenerateDataKeyResult GenerateDataKey(string keyId)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException("AwsKmsKeyManager");
			}

			ValidateMasterKey(keyId);

			var res = RetryPolicy.ExecuteAndCapture(() =>
			{
				var req = new GenerateDataKeyRequest
				{
					KeyId = keyId,
					KeySpec = DataKeySpec.AES_128
				};

				var t = _client.GenerateDataKeyAsync(req);
				t.ConfigureAwait(false);
				return t.GetAwaiter().GetResult();
			});

			ValidateResponse(res);

			var result = (GenerateDataKeyResponse)res.Result;
			return new GenerateDataKeyResult
			{
				KeyId = keyId,
				CipherTextKey = result.CiphertextBlob.ToArray(),
				PlainTextKey = result.Plaintext.ToArray()
			};
		}

		/// <summary>
		/// Decrypts the passed data with a master key. This method will decrypt a
		/// payload of up to 6144 bytes returning the resulting clear text
		/// </summary>
		/// <param name="data">The encrypted data to decrypt.</param>
		/// <returns>Resulting clear text.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentException">You must provide some data to encrypt</exception>
		/// <exception cref="PayloadTooLargeException"></exception>
		/// <exception cref="KeyManagementServiceUnavailableException"></exception>
		public byte[] DecryptData(byte[] data)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException("AwsKmsKeyManager");
			}

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

			var res = RetryPolicy.ExecuteAndCapture(() =>
			{
				using (var msData = new MemoryStream(data))
				{
					var req = new DecryptRequest
					{
						CiphertextBlob = msData
					};

					var t = _client.DecryptAsync(req);
					t.ConfigureAwait(false);
					return t.GetAwaiter().GetResult();
				}
			});

			ValidateResponse(res);

			return ((DecryptResponse)res.Result).Plaintext.ToArray();
		}

		private void ValidateMasterKey(string keyId)
		{
			if (keyId == null)
			{
				throw new ArgumentNullException(nameof(keyId));
			}

			if (String.IsNullOrWhiteSpace(keyId))
			{
				throw new ArgumentException("You must provide a Key ID", nameof(keyId));
			}
		}

		private void ValidateResponse(PolicyResult<AmazonWebServiceResponse> res)
		{
			if (res.Result == null )
			{
				if (res.FinalHandledResult == null && res.FinalException != null)
				{
					throw res.FinalException;
				}
				
				throw new KeyManagementServiceUnavailableException(res.FinalHandledResult?.HttpStatusCode);
			}

			if (res.Result.HttpStatusCode != HttpStatusCode.OK)
			{
				throw new KeyManagementServiceUnavailableException(res.Result.HttpStatusCode);
			}
		}

		[ExcludeFromCodeCoverage]
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		[ExcludeFromCodeCoverage]
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				_client?.Dispose();
				_client = null;
			}

			_disposed = true;
		}
	}
}
