using System;
using LeapingGorilla.SecretStore.Interfaces;

namespace LeapingGorilla.SecretStore
{
	public class SecretStore : ISecretStore
	{
		private readonly IProtectedSecretRepository _secrets;
		private readonly IEncryptionManager _encryptionManager;

		public SecretStore(IProtectedSecretRepository secrets, IEncryptionManager encryptionManager)
		{
			_secrets = secrets;
			_encryptionManager = encryptionManager;
		}


		public void Save(Secret secret)
		{
			//var ps = Protect(secret);

			throw new NotImplementedException();
		}

		public Secret Get(string name)
		{
			throw new NotImplementedException();
		}

		public ProtectedSecret Protect(string keyName, Secret secret)
		{
			throw new NotImplementedException();
		}

		public Secret Unprotect(ProtectedSecret protectedSecret)
		{
			throw new NotImplementedException();
		}
	}
}
