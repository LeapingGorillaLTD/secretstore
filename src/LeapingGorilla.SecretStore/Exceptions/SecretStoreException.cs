using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace LeapingGorilla.SecretStore.Exceptions
{
	///<summary>Root class that all exceptions in Secret Store will inherit from</summary>
	[ExcludeFromCodeCoverage]
	public class SecretStoreException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SecretStoreException"/> class.
		/// </summary>
		public SecretStoreException() : base("An exception has occurred within the Secret Store")
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SecretStoreException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public SecretStoreException(string message) : base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SecretStoreException"/> class.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
		public SecretStoreException(string message, Exception innerException) : base(message, innerException)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SecretStoreException"/> class.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
		protected SecretStoreException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
