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
	public class EncryptionResult
	{
		///<summary>Encrypted data key which was used to protect the data</summary>
		public byte[] EncryptedDataKey { get; set; }

		///<summary>The IV used for encryption</summary>
		public byte[] InitialisationVector { get; set; }

		///<summary>The encrypted data</summary>
		public byte[] EncryptedData { get; set; }

		public EncryptionResult() { }

		public EncryptionResult(byte[] iv, byte[] encryptedDataKey, byte[] encryptedData)
		{
			InitialisationVector = iv;
			EncryptedDataKey = encryptedDataKey;
			EncryptedData = encryptedData;
		}
	}
}
