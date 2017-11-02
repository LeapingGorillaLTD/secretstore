using System;
using System.Collections.Generic;
using System.Security;
using System.Threading;
using LeapingGorilla.SecretStore.Aws;
using LeapingGorilla.SecretStore.Interfaces;

namespace LeapingGorilla.SecretStore.CommandLine
{
    public class CommandImplementation
    {
	    private readonly ISecretStore _secretStore;
	    private readonly IProtectedSecretRepository _repo;

		public CommandImplementation(ISecretStore ss, IProtectedSecretRepository repo)
		{
			_repo = repo;
			_secretStore = ss;
		}

	    public IEnumerable<string> GetAllSecretsForApplication(string applicationName)
	    {
		    throw new NotImplementedException();
	    }

	    public string GetSecret(string application, string name)
	    {
		    return _secretStore.Get(application, name).Value;
	    }

	    public void AddSecret(string key, string application, string name, SecureString value)
	    {
		    var s = new Secret(application, name, value.ToUnprotectedString());
		    var ps = _secretStore.Protect(key, s);
		    _secretStore.Save(ps);
	    }

	    public void CreateTable(string tableName)
	    {
		    using (var cts = new CancellationTokenSource(TimeSpan.FromMinutes(1)))
		    {
			    ((AwsDynamoProtectedSecretRepository)_repo)
					.CreateProtectedSecretTableAsync(tableName)
					.Wait(cts.Token);
		    }
	    }
    }
}
