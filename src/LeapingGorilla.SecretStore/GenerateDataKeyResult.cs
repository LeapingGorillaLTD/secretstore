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
	public class GenerateDataKeyResult
	{
		///<summary>Plain Text key which you can use for encryption or decryption. This should be discarded after use</summary>
		public byte[] PlainTextKey { get; set; }

		///<summary>Plain text key which has been encrypted by a Master Key. This should be stored alongside the item you are encrypting</summary>
		public byte[] CipherTextKey { get; set; }

		///<summary>ID of the master key used to generate and protect this data key</summary>
		public string KeyId { get; set; }
	}
}
