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

namespace LeapingGorilla.SecretStore.Exceptions
{
	///<summary>
	/// Thrown if a named secret cannot be found in the backing data store. Secret names may
	/// be case sensitive dependant on the backing data store.
	/// </summary>
	public class SecretNotFoundException : SecretStoreException
	{
		public string ApplicationName { get; }
		public string SecretName { get; }

		public SecretNotFoundException(string applicationName, string secretName) : base($"No secret could be found with the name {secretName} for application {applicationName}. Your secret name may be case sensitive depending on your backing data store")
		{
			ApplicationName = applicationName;
			SecretName = secretName;
		}
	}
}
