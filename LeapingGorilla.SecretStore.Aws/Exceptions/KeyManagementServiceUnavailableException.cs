using System;
using System.Net;

namespace LeapingGorilla.SecretStore.Aws.Exceptions
{
	public class KeyManagementServiceUnavailableException : Exception
	{
		public HttpStatusCode LastStatusCode { get; private set; }

		public KeyManagementServiceUnavailableException(HttpStatusCode lastStatusCode) : base($"Failed to contact the Key Management Service. Last status code: '{lastStatusCode}'. See http://docs.aws.amazon.com/kms/latest/APIReference/API_Decrypt.html for failed status code meanings")
		{
			LastStatusCode = lastStatusCode;
		}
	}
}
