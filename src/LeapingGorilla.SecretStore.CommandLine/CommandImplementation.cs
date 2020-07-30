using System;
using System.Collections.Generic;
using System.Security;
using System.Threading;
using Amazon;
using Amazon.DynamoDBv2;
using LeapingGorilla.SecretStore.Aws;
using LeapingGorilla.SecretStore.Interfaces;

namespace LeapingGorilla.SecretStore.CommandLine
{
    public class CommandImplementation
    {
	    private ISecretStore _secretStore;
	    private IProtectedSecretRepository _repo;
		

	    public IEnumerable<ClearSecret> GetAllSecretsForApplication(string tableName, string applicationName)
	    {
			SetDependencies(tableName);
		    return _secretStore.GetAllForApplication(applicationName);
	    }

	    public string GetSecret(string tableName, string application, string name)
	    {
		    SetDependencies(tableName);
		    return _secretStore.Get(application, name).Value;
	    }

	    public void AddSecret(string tableName, string key, string application, string name, SecureString value)
	    {
		    SetDependencies(tableName);
		    var s = new ClearSecret(application, name, value.ToUnprotectedString());
		    var ps = _secretStore.Protect(key, s);
		    _secretStore.Save(ps);
	    }

	    public void CreateTable(string tableName)
	    {
		    SetDependencies(tableName);
		    if (_repo is ICreateProtectedSecretTable r)
		    {
				using (var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5)))
				{
					r.CreateProtectedSecretTableAsync(tableName).Wait(cts.Token);
				}
			}
		    else
		    {
				throw new ArgumentException("Repository does not implement ICreateProtectedSecretTable");
		    }
	    }

	    public void SetDependencies(string tableName)
	    {
		    var region = RegionEndpoint.EUWest1;
		    var km = new AwsKmsKeyManager(region);
		    var config = new AmazonDynamoDBConfig
		    {
			    RegionEndpoint = region
		    };

		    var repo = new AwsDynamoProtectedSecretRepository(config, tableName);
		    var em = new EncryptionManager(km);
		    var ss = new SecretStore(repo, em);

		    _secretStore = ss;
		    _repo = repo;
	    }

    }
}
