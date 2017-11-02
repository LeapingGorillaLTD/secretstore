using System;
using System.Text;
using System.Threading.Tasks;
using LeapingGorilla.SecretStore.Interfaces;

namespace LeapingGorilla.SecretStore
{
	public class SecretStore : ISecretStore
	{
		private readonly IProtectedSecretRepository _secrets;
		private readonly IEncryptionManager _encryptionManager;

		private static readonly Encoding SecretEncoding = Encoding.UTF8;

		public SecretStore(IProtectedSecretRepository secrets, IEncryptionManager encryptionManager)
		{
			_secrets = secrets;
			_encryptionManager = encryptionManager;
		}
		
		public void Save(string keyName, Secret secret)
		{
			var ps = Protect(keyName, secret);
			_secrets.Save(ps);
		}

		public Secret Get(string applicationName, string secretName)
		{
			return Unprotect(_secrets.Get(applicationName, secretName));
		}

		public async Task<Secret> GetAsync(string applicationName, string secretName)
		{
			return Unprotect(await _secrets.GetAsync(applicationName, secretName));
		}

		public ProtectedSecret Protect(string keyName, Secret secret)
		{
			if (secret == null)
			{
				throw new ArgumentNullException(nameof(secret));
			}

			var encryptedSecret = _encryptionManager.Encrypt(keyName, SecretEncoding.GetBytes(secret.Value));
			return new ProtectedSecret
			{
				MasterKeyId = keyName,
				InitialisationVector = encryptedSecret.InitialisationVector,
				Name = secret.SecretName,
				ProtectedDocumentKey = encryptedSecret.EncryptedDataKey,
				ProtectedSecretValue = encryptedSecret.EncryptedData
			};
		}

		public void Save(ProtectedSecret secret)
		{
			if (secret == null)
			{
				throw new ArgumentNullException(nameof(secret));
			}

			_secrets.Save(secret);
		}

		public async Task SaveAsync(ProtectedSecret secret)
		{
			if (secret == null)
			{
				throw new ArgumentNullException(nameof(secret));
			}

			await _secrets.SaveAsync(secret);
		}

		public Secret Unprotect(ProtectedSecret protectedSecret)
		{
			if (protectedSecret == null)
			{
				throw new ArgumentNullException(nameof(protectedSecret));
			}

			var rawValue = _encryptionManager.Decrypt(protectedSecret.MasterKeyId, protectedSecret.ProtectedDocumentKey, protectedSecret.InitialisationVector, protectedSecret.ProtectedSecretValue);
			return new Secret
			{
				SecretName = protectedSecret.Name,
				Value = SecretEncoding.GetString(rawValue)
			};
		}
	}
}
