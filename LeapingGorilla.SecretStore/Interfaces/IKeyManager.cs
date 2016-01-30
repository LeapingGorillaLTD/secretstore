namespace LeapingGorilla.SecretStore.Interfaces
{
	public interface IKeyManager
	{

		/// <summary>
		/// Encrypts the passed data with a Master Key. This method will encrypt 
		/// a payload up to a maximum size of 4096 bytes. The master key with the 
		/// specified ID will be used to encrypt the data.
		/// </summary>
		/// <param name="keyId">The identifier for the master key.</param>
		/// <param name="data">The data to encrypt.</param>
		/// <returns>Encrypted data.</returns>
		byte[] EncryptData(string keyId, byte[] data);

		/// <summary>
		/// Generates a data key for the specified master key. This will return both
		/// the plaintext key that you can use for encryption/decryption and the 
		/// same key as protected by the master key.
		/// </summary>
		/// <param name="keyId">
		/// The identifier of the key we should use to generate and protect the data key.
		/// </param>
		/// <returns>
		/// Result containing the plaintext representation of the generated data key
		/// which can be used for encryption/decryption, an encrypted version of the 
		/// data key encrypted by the master key and the ID of the key that was used.
		/// </returns>
		GenerateDataKeyResult GenerateDataKey(string keyId);

		/// <summary>
		/// Decrypts the passed data with a master key. This method will decrypt a 
		/// payload of up to 6144 bytes returning the resulting clear text
		/// </summary>
		/// <param name="encryptedData">The encrypted data to decrypt.</param>
		/// <returns>Resulting clear text.</returns>
		byte[] DecryptData(byte[] encryptedData);
	}
}
