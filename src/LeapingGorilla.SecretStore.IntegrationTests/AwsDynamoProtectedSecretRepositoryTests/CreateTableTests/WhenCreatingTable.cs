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

using System;
using Amazon;
using Amazon.DynamoDBv2;
using LeapingGorilla.SecretStore.Tests.Builders;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit.Attributes;

namespace LeapingGorilla.SecretStore.IntegrationTests.AwsDynamoProtectedSecretRepositoryTests.CreateTableTests
{
	public class WhenCreatingTable : WhenTestingAwsDynamoProtectedSecretRepository
	{
		private ProtectedSecret _savedSecret;
		private ProtectedSecret _retrievedSecret;

		[When]
		public async Task TableCreatedAndSecretAdded()
		{
			await Repository.CreateProtectedSecretTableAsync(TableName);
			_savedSecret = ProtectedSecretBuilder.Create().AnInstance();
			await Repository.SaveAsync(_savedSecret);
			_retrievedSecret = await Repository.GetAsync(_savedSecret.ApplicationName, _savedSecret.Name);
		}

		[Then]
		public void SecretShouldBeRetrieved()
		{
			Assert.That(_retrievedSecret, Is.Not.Null);
		}

		[Then]
		public void RetrievedSecretShouldBeAsExpected()
		{
			Assert.That(_retrievedSecret, Is.EqualTo(_savedSecret));
		}

		[OneTimeTearDown]
		public void DeleteTable()
		{
			var client = new AmazonDynamoDBClient(RegionEndpoint.EUWest1);
			client.DeleteTableAsync(TableName).Wait(TimeSpan.FromSeconds(30));
		}
	}
}