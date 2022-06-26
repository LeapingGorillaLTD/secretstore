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

using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit.Attributes;
using NSubstitute;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.CachingSecretStoreTests.CachingClearSecretStoreTests
{
	public class WhenCallingGetNotInCache : WhenTestingCachingClearSecretStore
	{
		private ClearSecret _returnedSecret;

		[Given]
		public void SecretNotInCache()
		{
			SecretCache.Get(Arg.Any<string>(), Arg.Any<string>()).Returns(default(ClearSecret));
		}

		[Given]
		public void SecretRepoGetReturnsProtectedSecret()
		{
			SecretRepository.Get(Arg.Any<string>(), Arg.Any<string>()).Returns(protectedSecret);
		}

		[Given]
		public void DecryptReturnsClearValue()
		{
			EncryptionManager
				.Decrypt(Arg.Any<string>(), Arg.Any<byte[]>(), Arg.Any<byte[]>(), Arg.Any<byte[]>())
				.Returns(BaseSecretStore.SecretEncoding.GetBytes(clearSecret.Value));
		}


		[When]
		public void GetCalled()
		{
			_returnedSecret = SecretStore.Get(clearSecret.ApplicationName, clearSecret.Name);
		}

		[Then]
		public void SecretReturned()
		{
			Assert.That(_returnedSecret, Is.Not.Null);
		}

		[Then]
		public void SecretAddedToCache()
		{
			SecretCache.Received(1).Add(_returnedSecret);
		}
	}
}
