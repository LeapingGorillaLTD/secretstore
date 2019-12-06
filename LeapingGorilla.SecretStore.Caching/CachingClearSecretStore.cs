using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeapingGorilla.SecretStore.Caching.Interfaces;
using LeapingGorilla.SecretStore.Interfaces;

namespace LeapingGorilla.SecretStore.Caching
{
	/// <summary>
	/// A secret store implementation which adds caching of secrets.
	/// THIS CLASS WILL PASS THE CLEAR TEXT OF YOUR SECRETS TO THE
	/// CACHE PROVIDER
	/// </summary>
	public class CachingClearSecretStore : BaseSecretStore, ISecretStore
	{
		private readonly ISecretCache<ClearSecret> _cache;

		public CachingClearSecretStore(
			IProtectedSecretRepository secrets, 
			IEncryptionManager encryptionManager,
			ISecretCache<ClearSecret> cache) :base(secrets, encryptionManager)
		{
			_cache = cache;
		}


		public void ProtectAndSave(string keyName, ClearSecret secret)
		{
			var ps = Protect(keyName, secret);
			_secrets.Save(ps);
			_cache.Add(secret);
		}

		public ClearSecret Get(string applicationName, string secretName)
		{
			var secret = _cache.Get(applicationName, secretName);

			if (secret == default(Secret))
			{
				secret = Unprotect(_secrets.Get(applicationName, secretName));
				_cache.Add(secret);
			}
			
			return secret;
		}

		public async Task<ClearSecret> GetAsync(string applicationName, string secretName)
		{
			var secret = await _cache.GetAsync(applicationName, secretName);

			if (secret == default(Secret))
			{
				secret = Unprotect(await _secrets.GetAsync(applicationName, secretName));
				await _cache.AddAsync(secret);
			}
			
			return secret;
		}

		public IEnumerable<ClearSecret> GetAllForApplication(string applicationName)
		{
			var secrets = _cache.GetAllForApplication(applicationName)?.ToList();

			if (secrets == null)
			{
				secrets = (_secrets.GetAllForApplication(applicationName) ?? Enumerable.Empty<ProtectedSecret>()).Select(Unprotect).ToList();
				_cache.AddMany(secrets);
			}

			return secrets;
		}

		public async Task<IEnumerable<ClearSecret>> GetAllForApplicationAsync(string applicationName)
		{
			var secrets = (await _cache.GetAllForApplicationAsync(applicationName))?.ToList();

			if (secrets == null)
			{
				secrets = (await _secrets.GetAllForApplicationAsync(applicationName) ?? Enumerable.Empty<ProtectedSecret>()).Select(Unprotect).ToList();
				await _cache.AddManyAsync(secrets);
			}

			return secrets;
		}

		public void Save(ProtectedSecret secret)
		{
			if (secret == null)
			{
				throw new ArgumentNullException(nameof(secret));
			}

			_secrets.Save(secret);
			_cache.Remove(secret.ApplicationName, secret.Name);
		}

		public async Task SaveAsync(ProtectedSecret secret)
		{
			if (secret == null)
			{
				throw new ArgumentNullException(nameof(secret));
			}

			await _secrets.SaveAsync(secret);
			await _cache.RemoveAsync(secret.ApplicationName, secret.Name);
		}
	}
}
