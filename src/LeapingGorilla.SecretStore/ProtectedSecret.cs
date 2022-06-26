// /*
//    Copyright 2013-2022 Leaping Gorilla LTD
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//        http://www.apache.org/licenses/LICENSE-2.0
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// */

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
