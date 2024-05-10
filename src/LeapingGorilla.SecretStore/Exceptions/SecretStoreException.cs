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
	}
}