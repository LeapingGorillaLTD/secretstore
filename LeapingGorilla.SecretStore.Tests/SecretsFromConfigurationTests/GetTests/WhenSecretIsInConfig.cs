using LeapingGorilla.Testing.Attributes;
using NSubstitute;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.SecretsFromConfigurationTests.GetTests
{
	public class WhenSecretIsInConfig : WhenTestingSecretsFromConfiguration
	{
		private string _key;
		private string _expectedName;
		private string _expectedApplication;
		private string _result;

		[Given]
		public void WeHaveHappyPath()
		{
			_key = "HappyPath";
			_expectedName = "HappyPathKey";
			_expectedApplication = "HappyPathApplication";

			SecretStore.Get(_expectedApplication, _expectedName).Returns(SuccessSecret);
		}

		[When]
		public void WeGetSecret()
		{
			_result = SecretsConfig.Get(_key);
		}

		[Then]
		public void ExpectedResultShouldBeReturned()
		{
			Assert.That(_result, Is.EqualTo(Success));
		}

		[Then]
		public void SecretStoreShouldBeCalledWithExpectedKey()
		{
			SecretStore.Received(1).Get(_expectedApplication, _expectedName);
		}
	}
}
