using System.Net;
using LeapingGorilla.SecretStore.Exceptions;

namespace LeapingGorilla.SecretStore.Aws.Exceptions
{
	public class KeyManagementServiceUnavailableException : SecretStoreException
	{
		public HttpStatusCode? LastStatusCode { get; }

		public KeyManagementServiceUnavailableException(HttpStatusCode? lastStatusCode) 
			: base($"Failed to contact the Key Management Service. Last status code: '{lastStatusCode}'. See http://docs.aws.amazon.com/kms/latest/APIReference/API_Decrypt.html for failed status code meanings")
		{
			LastStatusCode = lastStatusCode;
		}
	}
}
