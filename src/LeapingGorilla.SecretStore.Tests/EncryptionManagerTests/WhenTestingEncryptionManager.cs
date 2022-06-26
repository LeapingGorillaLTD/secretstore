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

using LeapingGorilla.SecretStore.Interfaces;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit;
using Newtonsoft.Json;
using NSubstitute;

namespace LeapingGorilla.SecretStore.Tests.EncryptionManagerTests
{
	public abstract class WhenTestingEncryptionManager : WhenTestingTheBehaviourOf
	{
		public const string ValidKeyId = "Test";

		[ItemUnderTest]
		public EncryptionManager Manager { get; set; }

		[Dependency]
		public IKeyManager KeyManager { get; set; }

		[Given]
		public void KeyManagerReturnsDataKey()
		{
			var dummy = DummyResult;
			KeyManager.GenerateDataKey(Arg.Is(ValidKeyId)).Returns(dummy);
			KeyManager.DecryptData(Arg.Any<byte[]>()).Returns(dummy.PlainTextKey);
		}

		private const string SerialisedDataKeyResult = "{\"PlainTextKey\":\"KdrFFWAcB4zeD493eqseJA==\",\"CipherTextKey\":\"gTDB2dht2kkvVE5XNjWMCBN0Wi+yCWO0ARmpfoLD3inyYTdiRVNvTfYBLs2+aEBuQhFI2inT8HRmVBRJWaz3G1Tw7ZzSv0IfsxLCaOHPjAXvxcyTlNJWsdSY8/C2XhdHPXBlTmaRR+FIOQ0D4GGFDnvQJTQU+c3ADIz093BC10l4R0WxgUqgl0xI85G4c7hcoIUiJhgYPsvadPgSEEwxXlvTh4B762g0g87U5kK3NJM0CnZ4GG/agFdEinyLDjDPA1uDBEgl+JPnuNlU1EyVdv6ev30XJc9NtS1verqKyNb5tCGjXd+7eLNc5FlVcfLs6Doo3/Z2qof+bVxR1IGDoGQJqcTynAau5ahFEH31CE/gntBu4hQVK2TBbFPJ7pzTy0GjgAPC4Ib12b92JGAC4aolvWRBlGyzT+gPL8mjXiiDW8tajr0s7+hWlJWXUrHQtKJ956jNYRay1Ribhi9T6ZRurpP6bH6Z0Wv6otrRmEZWqDsOXEuRMnL2M3mNVHOeiSLqo3F0E8Q1gLlF94szucvjL1cnE6y48qlPecBLhFNI4YvIdyETqnuLZqymL0Fn4y4sBSOUf4iSFgLgsLVIkg/nZyJ7CAjrXu3GzQK0xXDX9K8yUfvioZcu8k3MutAL4I2WqiDy7sNslIXex9PvpNky9LW4YeyLjTyYoRT9PHw=\",\"MasterKeyId\":\"Test\"}";
		private GenerateDataKeyResult DummyResult => JsonConvert.DeserializeObject<GenerateDataKeyResult>(SerialisedDataKeyResult);
	}
}
