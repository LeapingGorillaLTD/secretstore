﻿// /*
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

namespace LeapingGorilla.SecretStore.Interfaces
{
	public interface IRsaKeyManager : IKeyManager
	{
		/// <summary>
		/// Generates a key ID segment and prepends it to the passed protected data.
		/// </summary>
		/// <param name="keyId">The key identifier we are generating a segment for.</param>
		/// <param name="protectedData">The protected data.</param>
		/// <returns>Byte array containing a key ID and the protected data.</returns>
		/// <remarks>
		/// The key Id segment is a single contiguous byte array which encapsulates the following 
		/// data:
		/// 
		///  | Key SecretName Bytes Size | Sig Bytes Size | Sig | Key SecretName Bytes | Protected Data Bytes
		/// 
		/// The first 4 bytes (0 to 3) are an Int32 containing the number of bytes in the Key Id (keyIdSize)
		/// The second 4 bytes (4 to 7) are an Int32 containing the number of bytes in the signature (sigSize)
		/// The next sigSize bytes are the signature (8 to (8 + sigSize - 1))
		/// The next keyIdSize bytes are the keyId encoded as UTF8 ((8 + sigSize) to (8 + sigSize + keyIdSize - 1))
		/// The entire key ID segment size is 8 + keyIdSize + sigSize (keyIdSegmentSize)
		/// The remaining bytes are the protected data (keyIdSegmentSize to (protectedData.Length - keyIdSegmentSize - 1))
		/// </remarks>
		byte[] PrependKeyIdSegment(string keyId, byte[] protectedData);

		/// <summary>
		/// Gets the key identifier from protected data. This extracts the key ID segment
		/// and verifies it using a signature. On completion it returns the key Id used to 
		/// protect the data and the offset at which the protected data begins
		/// </summary>
		/// <param name="protectedData">The protected data we will extract the key ID form.</param>
		/// <param name="keyIdSegmentLength">
		/// Length of the keyname segment. This can be used as an offset for the start of 
		/// the protected data
		/// </param>
		/// <returns>The ID of the key that the protected data was encrypted with.</returns>
		string GetKeyId(byte[] protectedData, out int keyIdSegmentLength);
	}
}
