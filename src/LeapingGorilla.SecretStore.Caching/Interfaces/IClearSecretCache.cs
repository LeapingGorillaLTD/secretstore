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

using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeapingGorilla.SecretStore.Caching.Interfaces
{
	public interface ISecretCache<T> where T: Secret
	{
		void Add(T secret);
		Task AddAsync(T secret);

		T Get(string applicationName, string secretName);
		Task<T> GetAsync(string applicationName, string secretName);

		IEnumerable<T> GetAllForApplication(string applicationName);
		Task<IEnumerable<T>> GetAllForApplicationAsync(string applicationName);

		void AddMany(IEnumerable<T> secrets);
		Task AddManyAsync(IEnumerable<T> secrets);

		void Remove(string applicationName, string secretName);
		Task RemoveAsync(string applicationName, string secretName);
	}
}
