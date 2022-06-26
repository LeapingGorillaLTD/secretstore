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

namespace LeapingGorilla.SecretStore.Exceptions
{
	///<summary>Thrown if the payload which is provided for encryption is too large to be encrytpted</summary>
	/// <remarks>This is mainly relevant to RSA encryption where the max encryptable payload is a function of the key size</remarks>
	public class PayloadTooLargeException : SecretStoreException
	{
		public int ProvidedPayloadSize { get; private set; }
		public int MaxPayloadSize { get; private set; }

		public PayloadTooLargeException(int maxSize, int providedSize) : base($"The payload you provided was larger than the maximum permitted. You passed: {providedSize} bytes, Max: {maxSize} bytes")
		{
			ProvidedPayloadSize = providedSize;
			MaxPayloadSize = maxSize;
		}
	}
}
