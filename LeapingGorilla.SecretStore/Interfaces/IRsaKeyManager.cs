namespace LeapingGorilla.SecretStore.Interfaces
{
	public interface IRsaKeyManager : IKeyManager
	{
		/// <summary>
		/// Generates a key name segment and prepends it to the passed protected data.
		/// </summary>
		/// <param name="keyId">The key identifier we are generating a segment for.</param>
		/// <param name="protectedData">The protected data.</param>
		/// <returns>Byte array containing a key ID and the protected data.</returns>
		byte[] PrependKeyIdSegment(string keyId, byte[] protectedData);

		/// <summary>
		/// Gets the key identifier from protected data. This extracts the key name block
		/// and verifies it using a signature.
		/// </summary>
		/// <param name="protectedData">The protected data we will extract the key ID form.</param>
		/// <param name="keyIdSegmentLength">Length of the keyname segment.</param>
		/// <returns>The ID of the key that the protected data was encrypted with.</returns>
		string GetKeyId(byte[] protectedData, out int keyIdSegmentLength);
	}
}
