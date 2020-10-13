using LeapingGorilla.SecretStore.Database.PostgreSQL;
using LeapingGorilla.SecretStore.Tests.Builders;
using LeapingGorilla.Testing.Attributes;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.IntegrationTests.DbProtectedRepositoryTests.PostgreSQLTests.SecretTests
{
	public class WhenSecretUpdatedInPlace : WhenTestingSecrets
	{
		private ProtectedSecret savedSecret;
		private ProtectedSecret returnedSecret;
		private ProtectedSecret returnedEditedSecret;
		private string editedMasterKeyId;

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

		[Given]
		public void WeHaveEditedMasterKeyId()
		{
			editedMasterKeyId = "NewMasterKey";
		}

		[When(DoNotRethrowExceptions: true)]
		public void WhenWeSaveThenRetrieveSecret()
		{
			Repository.Save(savedSecret);
			returnedSecret = Repository.Get(savedSecret.ApplicationName, savedSecret.Name);

			savedSecret.MasterKeyId = editedMasterKeyId;
			Repository.Save(savedSecret);
			returnedEditedSecret = Repository.Get(savedSecret.ApplicationName, savedSecret.Name);
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
			Assert.That(returnedSecret.MasterKeyId, Is.EqualTo(ProtectedSecretBuilder.Defaults.MasterKeyId));
		}
		
		[Then]
		public void EditedSecretShouldHaveExpectedSecretValue()
		{
			Assert.That(returnedEditedSecret.ProtectedSecretValue, Is.EqualTo(savedSecret.ProtectedSecretValue));
		}
		
		[Then]
		public void EditedSecretShouldHaveExpectedIV()
		{
			Assert.That(returnedEditedSecret.InitialisationVector, Is.EqualTo(savedSecret.InitialisationVector));
		}
		
		[Then]
		public void EditedSecretShouldHaveExpectedDocumentKey()
		{
			Assert.That(returnedEditedSecret.ProtectedDocumentKey, Is.EqualTo(savedSecret.ProtectedDocumentKey));
		}
		
		[Then]
		public void EditedSecretShouldHaveExpectedMasterKey()
		{
			Assert.That(returnedEditedSecret.MasterKeyId, Is.EqualTo(editedMasterKeyId));
		}
	}
}
