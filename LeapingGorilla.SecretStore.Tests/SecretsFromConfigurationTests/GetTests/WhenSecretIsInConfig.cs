using LeapingGorilla.Testing.Attributes;
using NSubstitute;
using NUnit.Framework;

namespace LeapingGorilla.SecretStore.Tests.SecretsFromConfigurationTests.GetTests
{
	public class WhenSecretIsInConfig : WhenTestingSecretsFromConfiguration
	{
		private string _name;
		private string _expectedKey;
		private string _result;

		[Given]
		public void WeHaveHappyPath()
		{
			_name = "HappyPath";
			_expectedKey = "HappyPathKey";
			SecretStore.Get(_expectedKey).Returns(SuccessSecret);
		}

		[When]
		public void WeGetSecret()
		{
			_result = SecretsConfig.Get(_name);
		}

		[Then]
		public void ExpectedResultShouldBeReturned()
		{
			Assert.That(_result, Is.EqualTo(Success));
		}

		[Then]
		public void SecretStoreShouldBeCalledWithExpectedKey()
		{
			SecretStore.Received(1).Get(_expectedKey);
		}
	}
}
