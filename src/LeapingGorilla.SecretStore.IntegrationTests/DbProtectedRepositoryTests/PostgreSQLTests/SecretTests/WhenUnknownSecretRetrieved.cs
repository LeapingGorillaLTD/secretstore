using LeapingGorilla.SecretStore.Exceptions;
using LeapingGorilla.Testing.Attributes;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.IntegrationTests.DbProtectedRepositoryTests.PostgreSQLTests.SecretTests
{
	public class WhenUnknownSecretRetrieved : WhenTestingSecrets
	{
		private ProtectedSecret secret;
		private string appName;
		private string secretName;

		[Given]
		public void WeHaveApplicationNameAndSecretName()
		{
			appName = "TestApp";
			secretName = "DoesNotExist";
		}

		[When(DoNotRethrowExceptions: true)]
		public void WhenGettingNonExistentSecret()
		{
			secret = Repository.Get(appName, secretName);
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
