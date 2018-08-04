using System;
using System.Collections.Generic;
using System.Security;
using System.Threading;
using LeapingGorilla.SecretStore.Interfaces;

namespace LeapingGorilla.SecretStore.CommandLine
{
    public class CommandImplementation
    {
	    private ISecretStore _secretStore;
	    private IProtectedSecretRepository _repo;
		

	    public IEnumerable<Secret> GetAllSecretsForApplication(string applicationName)
	    {
		    return _secretStore.GetAllForApplication(applicationName);
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
		    if (_repo is ICreateProtectedSecretTable r)
		    {
				using (var cts = new CancellationTokenSource(TimeSpan.FromMinutes(1)))
				{
					r.CreateProtectedSecretTableAsync(tableName).Wait(cts.Token);
				}
			}
	    }

	    public void SetDependencies(ISecretStore ss, IProtectedSecretRepository repo)
	    {
		    _repo = repo;
			_secretStore = ss;
	    }
    }
}
