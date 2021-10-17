using System.Threading.Tasks;
using LeapingGorilla.SecretStore.Exceptions;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit.Attributes;
using NUnit.Framework;

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
