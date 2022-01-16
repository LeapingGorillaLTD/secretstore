using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeapingGorilla.SecretStore.Caching.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace LeapingGorilla.SecretStore.Caching
{
	public class InMemoryCache<T> : ISecretCache<T> where T : Secret
	{
		public TimeSpan SecretAge = TimeSpan.FromMinutes(10);

		public MemoryCache Cache { get; }

		private string CachePrefix { get; }

		private HashSet<string> Keys { get; }

		/// <summary>
		/// Default options applied to all secrets added to the cache. If none
		/// supplied in the constructor we will use an absolute relative expiry
		/// equal to <see cref="SecretAge"/> and a priority of "Normal"
		/// </summary>
        public MemoryCacheEntryOptions CacheEntryOptions { get; }

		/// <summary>
		/// Create a new InMemoryCache using the given prefix and cache entry options
		/// </summary>
		/// <param name="cachePrefix">(Optional) Prefix which will be added to every cache key</param>
		/// <param name="cacheEntryOptions">
		/// (Optional) Default options applied to all secrets added to the cache. If none
        /// supplied we will use an absolute relative expiry equal to <see cref="SecretAge"/>
        /// and a priority of "Normal"
		/// </param>
		public InMemoryCache(
            string cachePrefix = null, 
            MemoryCacheEntryOptions cacheEntryOptions = null)
		{
			CachePrefix = cachePrefix;
			Cache = new MemoryCache(new MemoryCacheOptions
			{
				ExpirationScanFrequency = TimeSpan.FromMinutes(1)
			});
			Keys = new HashSet<string>();

            CacheEntryOptions = cacheEntryOptions ?? new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = SecretAge,
                Priority = CacheItemPriority.Normal
            };
        }

		public void Add(T secret)
		{
			var key = GenerateCacheKey(secret);
			Cache.Set(key, secret, CacheEntryOptions);
		}

		public Task AddAsync(T secret)
		{
			Add(secret);
			return Task.CompletedTask;
		}

		public T Get(string applicationName, string secretName)
		{
			return Cache.Get<T>(GenerateCacheKey(applicationName, secretName));
		}

		public Task<T> GetAsync(string applicationName, string secretName)
		{
			return Task.FromResult(Get(applicationName, secretName));
		}

		public IEnumerable<T> GetAllForApplication(string applicationName)
		{
			var searchString = CachePrefix == null
								? $"{applicationName}:"
								: $"{CachePrefix}:{applicationName}:";

			var cachedSecrets = new List<T>();
			foreach (var possibleKey in Keys.Where(k => k.StartsWith(searchString)))
			{
				var secret = Cache.Get<T>(possibleKey);
				if (secret != null)
				{
					cachedSecrets.Add(secret);
				}
			}

			if (cachedSecrets.Any())
			{
				return cachedSecrets;
			}

			return null;
		}

		public Task<IEnumerable<T>> GetAllForApplicationAsync(string applicationName)
		{
			return Task.FromResult(GetAllForApplication(applicationName));
		}

		public void AddMany(IEnumerable<T> secrets)
		{
			foreach (var secret in secrets)
			{
				Add(secret);
			}
		}

		public Task AddManyAsync(IEnumerable<T> secrets)
		{
			AddMany(secrets);
			return Task.CompletedTask;
		}

		public void Remove(string applicationName, string secretName)
		{
			var key = GenerateCacheKey(applicationName, secretName);
			Cache.Remove(key);
			Keys.Remove(key);
		}

		public Task RemoveAsync(string applicationName, string secretName)
		{
			Remove(applicationName, secretName);
			return Task.CompletedTask;
		}

		private string GenerateCacheKey(T secret)
		{
			return GenerateCacheKey(secret.ApplicationName, secret.Name);
		}
		
		private string GenerateCacheKey(string applicationName, string name)
		{
            var key = CachePrefix != null 
                        ? $"{CachePrefix}:{applicationName}:{name}" 
                        : $"{applicationName}:{name}";

			Keys.Add(key);
			return key;
		}
	}
}
