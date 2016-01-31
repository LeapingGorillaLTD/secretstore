using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeapingGorilla.SecretStore.Interfaces
{
	public interface ISecretStore
	{
		void Save(Secret secret);

		Secret Get(string name);

		ProtectedSecret Protect(Secret secret);

		Secret Unprotect(ProtectedSecret protectedSecret);
	}
}
