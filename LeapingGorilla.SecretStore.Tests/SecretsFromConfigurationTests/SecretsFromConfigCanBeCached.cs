using System;
using System.Runtime.Caching;
using LeapingGorilla.SecretStore.Configuration;
using LeapingGorilla.Testing.Attributes;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.SecretsFromConfigurationTests
{
	public class SecretsFromConfigCanBeCached : WhenTestingSecretsFromConfiguration
	{
		private Exception _ex;
		private MemoryCache _cache;
		private string _cacheKey;
		private SecretsFromConfiguration _result;

		[Given]
		public void WeHaveInMemoryCache()
		{
			_cache = new MemoryCache("SecretsFromConfigCanBeCached.Test");
			_cacheKey = "TestKey";
		}

		[When]
		public void SecretStoreIsCached()
		{
			try
			{
				_cache.Add(_cacheKey, SecretsConfig, DateTime.Now.AddSeconds(5));
				_result = _cache[_cacheKey] as SecretsFromConfiguration;
			}
			catch (Exception ex)
			{
				_ex = ex;
			}
		}

		[Then]
		public void NoExceptionShouldOccur()
		{
			Assert.That(_ex, Is.Null);
		}

		[Then]
		public void ResultShouldBeReturnedFromCache()
		{
			Assert.That(_result, Is.Not.Null);
		}
	}
}
