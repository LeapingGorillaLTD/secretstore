namespace LeapingGorilla.SecretStore.Interfaces
{
	public interface IEncryptionManager
	{
		/// <summary>
		/// Encrypts a given byte array with a generated data key then encrypts the
		/// data key with a Master Key. Returns a result containing the encrypted
		/// data and the encrypted data key.
		/// </summary>
		/// <param name="keyId">The ID of the master key we will use for protecting the data key.</param>
		/// <param name="dataToEncrypt">Data to be encrypted</param>
		/// <returns>Result containing the encrypted data and the encrypted master key</returns>
		EncryptionResult Encrypt(string keyId, byte[] dataToEncrypt);

		/// <summary>
		/// Decrypts the passed data key with the master key and then uses
		/// it to decrypt the passed encrypted data.
		/// </summary>
		/// <param name="keyId">The id of the Master Key used to encrypt the data.</param>
		/// <param name="encryptedDataKey">Data Key that has been encrypted with a Master Key</param>
		/// <param name="iv"></param>
		/// <param name="encryptedData">Data to be decrypted</param>
		/// <returns>The decrypted data</returns>
		byte[] Decrypt(string keyId, byte[] encryptedDataKey, byte[] iv, byte[] encryptedData);
	}
}
