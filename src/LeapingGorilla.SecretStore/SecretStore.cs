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
