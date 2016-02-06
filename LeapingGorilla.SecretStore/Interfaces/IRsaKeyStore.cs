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
		/// <summary>
		/// Gets the public key for the key with the given ID.
		/// </summary>
		/// <param name="keyId">The key identifier.</param>
		/// <returns>RSAParameters for the public key with the given name.</returns>
		RSAParameters GetPublicKey(string keyId);

		/// <summary>
		/// Gets the private key for the key with the specified ID.
		/// </summary>
		/// <param name="keyId">The key identifier.</param>
		/// <returns>RSAParameters for the private key with the given name.</returns>
		RSAParameters GetPrivateKey(string keyId);

		/// <summary>
		/// Gets the key which is used for signing requests. This should be a 
		/// separate key which is always available.
		/// </summary>
		/// <returns>RSAParameters for a signing key.</returns>
		RSAParameters GetSigningKey();
	}
}
