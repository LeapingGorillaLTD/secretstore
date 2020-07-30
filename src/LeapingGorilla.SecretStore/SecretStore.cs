using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeapingGorilla.SecretStore.Interfaces;

namespace LeapingGorilla.SecretStore
{
	public class SecretStore : BaseSecretStore, ISecretStore
	{
		public SecretStore(IProtectedSecretRepository secrets, IEncryptionManager encryptionManager)
			:base(secrets, encryptionManager)
		{
		}
		
		public void ProtectAndSave(string keyName, ClearSecret secret)
		{
			var ps = Protect(keyName, secret);
			_secrets.Save(ps);
		}

		public ClearSecret Get(string applicationName, string secretName)
		{
			return Unprotect(_secrets.Get(applicationName, secretName));
		}

		public async Task<ClearSecret> GetAsync(string applicationName, string secretName)
		{
			return Unprotect(await _secrets.GetAsync(applicationName, secretName));
		}

		public IEnumerable<ClearSecret> GetAllForApplication(string applicationName)
		{
			return _secrets.GetAllForApplication(applicationName).Select(Unprotect).ToList();
		}

		public async Task<IEnumerable<ClearSecret>> GetAllForApplicationAsync(string applicationName)
		{
			var s = await _secrets.GetAllForApplicationAsync(applicationName);
			return s.Select(Unprotect).ToList();
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
	}
}
