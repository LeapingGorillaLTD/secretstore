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

using LeapingGorilla.SecretStore.Database.PostgreSQL;
using LeapingGorilla.SecretStore.Tests.Builders;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit.Attributes;

namespace LeapingGorilla.SecretStore.IntegrationTests.DbProtectedRepositoryTests.PostgreSQLTests.ConstructorTests
{
	public class WhenUsingMultipleConnStrAndRespectingAccess : WhenTestingPostgreSQLDbRepository
	{
		private ProtectedSecret savedSecret;
		private ProtectedSecret returnedSecret;

		protected override void CreateManualDependencies()
		{
			base.CreateManualDependencies();
			Repository = new PostgreSQLDbProtectedRepository(ReadOnlyConnectionString, ReadWriteConnectionString, TableName);
		}

		[Given]
		public void WeHaveSecretToSave()
		{
			savedSecret = ProtectedSecretBuilder.Create().AnInstance();
		}

		[When(DoNotRethrowExceptions: true)]
		public async Task WhenWeSaveThenRetrieveSecret()
		{
			await Repository.CreateProtectedSecretTableAsync(TableName);
			Repository.Save(savedSecret);
			returnedSecret = Repository.Get(savedSecret.ApplicationName, savedSecret.Name);
		}

		[Then]
		public void NoExceptionThrown()
		{
			Assert.That(ThrownException, Is.Null);
		}

		[Then]
		public void ReturnedSecretShouldHaveExpectedSecretValue()
		{
			Assert.That(returnedSecret.ProtectedSecretValue, Is.EqualTo(savedSecret.ProtectedSecretValue));
		}
		
		[Then]
		public void ReturnedSecretShouldHaveExpectedIv()
		{
			Assert.That(returnedSecret.InitialisationVector, Is.EqualTo(savedSecret.InitialisationVector));
		}
		
		[Then]
		public void ReturnedSecretShouldHaveExpectedDocumentKey()
		{
			Assert.That(returnedSecret.ProtectedDocumentKey, Is.EqualTo(savedSecret.ProtectedDocumentKey));
		}
		
		[Then]
		public void ReturnedSecretShouldHaveExpectedMasterKey()
		{
			Assert.That(returnedSecret.MasterKeyId, Is.EqualTo(savedSecret.MasterKeyId));
		}
	}
}
