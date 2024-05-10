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

using LeapingGorilla.SecretStore.Exceptions;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit.Attributes;

namespace LeapingGorilla.SecretStore.IntegrationTests.DbProtectedRepositoryTests.PostgreSQLTests.SecretTests
{
	public class WhenUnknownSecretRetrievedAsync : WhenTestingSecrets
	{
		private ProtectedSecret secret;private string appName;
		private string secretName;

		[Given]
		public void WeHaveApplicationNameAndSecretName()
		{
			appName = "TestApp";
			secretName = "DoesNotExist";
		}

		[When(DoNotRethrowExceptions: true)]
		public async Task WhenGettingNonExistentSecret()
		{
			secret = await Repository.GetAsync(appName, secretName);
		}

		[Then]
		public void ExceptionThrown()
		{
			Assert.That(ThrownException, Is.Not.Null);
		}

		[Then]
		public void NoSecretReturned()
		{
			Assert.That(secret, Is.Null);
		}

		[Then]
		public void ExceptionIsExpectedType()
		{
			Assert.That(ThrownException, Is.TypeOf<SecretNotFoundException>());
		}

		[Then]
		public void ExceptionHasCorrectApplicationName()
		{
			Assert.That(((SecretNotFoundException)ThrownException).ApplicationName, Is.EqualTo(appName));
		}
		
		[Then]
		public void ExceptionHasCorrectSecretName()
		{
			Assert.That(((SecretNotFoundException)ThrownException).SecretName, Is.EqualTo(secretName));
		}
	}
}
