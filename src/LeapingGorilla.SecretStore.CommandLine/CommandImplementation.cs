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
