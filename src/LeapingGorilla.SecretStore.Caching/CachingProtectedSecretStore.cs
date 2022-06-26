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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeapingGorilla.SecretStore.Caching.Interfaces;
using LeapingGorilla.SecretStore.Interfaces;

namespace LeapingGorilla.SecretStore.Caching
{
	/// <summary>
	/// A secret store implementation which adds caching of protected secrets
	/// </summary>
	public class CachingProtectedSecretStore : BaseSecretStore, ISecretStore
	{
		private readonly ISecretCache<ProtectedSecret> _cache;

		public CachingProtectedSecretStore(IProtectedSecretRepository secrets, 
			IEncryptionManager encryptionManager,
			ISecretCache<ProtectedSecret> cache) :base(secrets, encryptionManager)
		{
			_cache = cache;
		}


		public void ProtectAndSave(string keyName, ClearSecret secret)
		{
			var ps = Protect(keyName, secret);
			_secrets.Save(ps);
			_cache.Add(ps);
		}

		public ClearSecret Get(string applicationName, string secretName)
		{
			var secret = _cache.Get(applicationName, secretName);

			if (secret == default(ProtectedSecret))
			{
				secret = _secrets.Get(applicationName, secretName);
				_cache.Add(secret);
			}
			
			return Unprotect(secret);
		}

		public async Task<ClearSecret> GetAsync(string applicationName, string secretName)
		{
			var secret = await _cache.GetAsync(applicationName, secretName);

			if (secret == default(ProtectedSecret))
			{
				secret = await _secrets.GetAsync(applicationName, secretName);
				await _cache.AddAsync(secret);
			}
			
			return Unprotect(secret);
		}

		public IEnumerable<ClearSecret> GetAllForApplication(string applicationName)
		{
			var secrets = _cache.GetAllForApplication(applicationName)?.ToList();

			if (secrets == null)
			{
				secrets = (_secrets.GetAllForApplication(applicationName) ?? Enumerable.Empty<ProtectedSecret>()).ToList();
				_cache.AddMany(secrets);
			}

			return secrets.Select(Unprotect).ToList();
		}

		public async Task<IEnumerable<ClearSecret>> GetAllForApplicationAsync(string applicationName)
		{
			var secrets = (await _cache.GetAllForApplicationAsync(applicationName))?.ToList();

			if (secrets == null)
			{
				secrets = (await _secrets.GetAllForApplicationAsync(applicationName) ?? Enumerable.Empty<ProtectedSecret>()).ToList();
				await _cache.AddManyAsync(secrets);
			}

			return secrets.Select(Unprotect).ToList();
		}

		public void Save(ProtectedSecret secret)
		{
			if (secret == null)
			{
				throw new ArgumentNullException(nameof(secret));
			}

			_secrets.Save(secret);
			_cache.Add(secret);
		}

		public async Task SaveAsync(ProtectedSecret secret)
		{
			if (secret == null)
			{
				throw new ArgumentNullException(nameof(secret));
			}

			await _secrets.SaveAsync(secret);
			await _cache.AddAsync(secret);
		}
	}
}
