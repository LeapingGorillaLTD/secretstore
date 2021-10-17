using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit.Attributes;
using NSubstitute;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.CachingSecretStoreTests.CachingClearSecretStoreTests
{
	public class WhenProtectingAndSaving : WhenTestingCachingClearSecretStore
	{
		private ProtectedSecret _savedSecret;

		[Given]
		public void ProtectProtects()
		{
			EncryptionManager
				.Encrypt(Arg.Any<string>(), Arg.Any<byte[]>())
				.Returns(new EncryptionResult(
					protectedSecret.InitialisationVector, 
					protectedSecret.ProtectedDocumentKey, 
					protectedSecret.ProtectedSecretValue));
		}

		[Given]
		public void SaveRecordsTheSecret()
		{
			SecretRepository
				.When(s => s.Save(Arg.Any<ProtectedSecret>()))
				.Do(ci => _savedSecret = ci.Arg<ProtectedSecret>());
		}

		[When]
		public void ProtectAndSaveCalled()
		{
			SecretStore.ProtectAndSave(keyName, clearSecret);
		}

		[Then]
		public void UnprotectedSecretAddedToCache()
		{
			SecretCache.Received(1).Add(clearSecret);
		}

		[Then]
		public void ProtectedSecretSaved()
		{
			SecretRepository.Received(1).Save(_savedSecret);
		}

		[Then]
		public void SavedSecretHasExpectedFields()
		{
			Assert.That(_savedSecret.InitialisationVector, Is.EqualTo(protectedSecret.InitialisationVector));
			Assert.That(_savedSecret.ProtectedDocumentKey, Is.EquivalentTo(protectedSecret.ProtectedDocumentKey));
			Assert.That(_savedSecret.ProtectedSecretValue, Is.EquivalentTo(protectedSecret.ProtectedSecretValue));
			Assert.That(_savedSecret.MasterKeyId, Is.EqualTo(protectedSecret.MasterKeyId));
		}
	}
}
