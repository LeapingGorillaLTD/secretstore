namespace LeapingGorilla.SecretStore
{
	///<summary>
	/// Models a secret protected with a document key. The document key itself is protected by 
	/// a master key.
	/// </summary>
	public class ProtectedSecret : Secret
	{
		///<summary>ID of the master key used to protect the document key protecting this secret</summary>
		public string MasterKeyId { get; set; }

		///<summary>
		/// Document Key used to protect this secret. The key is encrypted using the master
		/// key identified in <see cref="MasterKeyId"/>
		///</summary>
		public byte[] ProtectedDocumentKey { get; set; }

		///<summary>The initialisation vector used to protect the value of this secret</summary>
		public byte[] InitialisationVector { get; set; }

		///<summary>
		/// Encrypted value for this secret protected with the Document Key held in 
		/// <see cref="ProtectedDocumentKey"/> and the IV from <see cref="InitialisationVector"/>
		///</summary>
		public byte[] ProtectedSecretValue { get; set; }
	}
}
