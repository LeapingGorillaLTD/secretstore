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
