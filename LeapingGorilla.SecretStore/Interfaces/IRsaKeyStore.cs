using System.Security.Cryptography;

namespace LeapingGorilla.SecretStore.Interfaces
{
	///<summary>
	/// Retrieves an RSA Public or Private key for use by the Key Manager.
	/// Note that the RSA key should have a length of at least 4096 bits so
	/// that we can support the maximum constraint on the Key Manager 
	/// interface
	/// </summary>
	public interface IRsaKeyStore
	{
		RSAParameters GetPublicKey(string keyId);

		RSAParameters GetPrivateKey(string keyId);
	}
}
