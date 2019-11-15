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

		protected static readonly Encoding SecretEncoding = Encoding.UTF8;

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
