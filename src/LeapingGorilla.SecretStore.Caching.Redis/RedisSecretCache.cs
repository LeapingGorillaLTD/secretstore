using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LeapingGorilla.SecretStore.Caching.Interfaces;
using StackExchange.Redis;

namespace LeapingGorilla.SecretStore.Caching.Redis
{
	public class RedisSecretCache<T> : ISecretCache<T> where T : Secret
	{
		private readonly ConnectionMultiplexer _redis;

		private IDatabase Cache => _redis.GetDatabase();

		public TimeSpan SecretAge = TimeSpan.FromMinutes(10);
		
		private string CachePrefix { get; }

		private HashSet<string> Keys { get; }

		private readonly JsonSerializerOptions _options = new JsonSerializerOptions
		{
			WriteIndented = false
		};

		public RedisSecretCache(string connectionString, string cachePrefix = null, TextWriter redisLogOutput = null)
		{
			CachePrefix = cachePrefix;
			_redis = ConnectionMultiplexer.Connect(connectionString, redisLogOutput);
			Keys = new HashSet<string>();
		}

		public void Add(T secret)
		{
			var key = GenerateCacheKey(secret);
			var body = Serialise(secret);
			Cache.StringSet(key, body, SecretAge);
		}

		public async Task AddAsync(T secret)
		{
			var key = GenerateCacheKey(secret);
			var body = await SerialiseAsync(secret);
			await Cache.StringSetAsync(key, body, SecretAge);
		}

		public T Get(string applicationName, string secretName)
		{
			var serialisedSecret = Cache.StringGet(GenerateCacheKey(applicationName, secretName));
			return Deserialize(serialisedSecret);
		}

		public async Task<T> GetAsync(string applicationName, string secretName)
		{
			var serialisedSecret = await Cache.StringGetAsync(GenerateCacheKey(applicationName, secretName));
			return await DeserializeAsync(serialisedSecret);
		}

		public IEnumerable<T> GetAllForApplication(string applicationName)
		{
			var searchString = CachePrefix == null
								? $"{applicationName}:"
								: $"{CachePrefix}:{applicationName}:";

			var cachedSecrets = new List<T>();
			foreach (var possibleKey in Keys.Where(k => k.StartsWith(searchString)))
			{
				var secret = Cache.StringGet(possibleKey);
				if (secret != RedisValue.EmptyString && secret != RedisValue.Null)
				{
					cachedSecrets.Add(Deserialize(secret));
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

		public async Task AddManyAsync(IEnumerable<T> secrets)
		{
			foreach (var secret in secrets)
			{
				await AddAsync(secret);
			}
		}

		public void Remove(string applicationName, string secretName)
		{
			var key = GenerateCacheKey(applicationName, secretName);
			Cache.KeyDelete(key);
			Keys.Remove(key);
		}

		public async Task RemoveAsync(string applicationName, string secretName)
		{
			var key = GenerateCacheKey(applicationName, secretName);
			await Cache.KeyDeleteAsync(key);
			Keys.Remove(key);
		}

		private string GenerateCacheKey(T secret)
		{
			return GenerateCacheKey(secret.ApplicationName, secret.Name);
		}
		
		private string GenerateCacheKey(string applicationName, string name)
		{
			string key;
			if (CachePrefix != null)
			{
				key = $"{CachePrefix}:{applicationName}:{name}";
			}
			else
			{
				key = $"{applicationName}:{name}";
			}

			Keys.Add(key);
			return key;
		}

		private string Serialise(T secret)
		{
			return JsonSerializer.Serialize(secret, _options);
		}
		
		private async Task<string> SerialiseAsync(T secret)
		{
			await using var ms = new MemoryStream();
			await JsonSerializer.SerializeAsync(ms, secret, _options);
			return Encoding.UTF8.GetString(ms.ToArray());
		}

		private T Deserialize(string serialisedSecret)
		{
			return JsonSerializer.Deserialize<T>(serialisedSecret, _options);
		}
		
		private async Task<T> DeserializeAsync(string serialisedSecret)
		{
			await using var ms = new MemoryStream(Encoding.UTF8.GetBytes(serialisedSecret));
			return await JsonSerializer.DeserializeAsync<T>(ms, _options);
		}
	}
}
