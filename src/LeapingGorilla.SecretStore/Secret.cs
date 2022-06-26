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

namespace LeapingGorilla.SecretStore
{
	///<summary>Models the values common to any secret</summary>
	public abstract class Secret
	{
		///<summary>Name of the application that the secret belongs to</summary>
		public string ApplicationName { get; set; }
		
		///<summary>Name of the secret</summary>
		public string Name { get; set; }
		
		protected Secret() {}

		/// <summary>
		/// Instantiates a new Secret with the common properties of application name
		/// and the name of the secret
		/// </summary>
		/// <param name="applicationName">Name of the application that the secret belongs to</param>
		/// <param name="name">Name of the secret</param>
		protected Secret(string applicationName, string name)
		{
			ApplicationName = applicationName;
			Name = name;
		}
	}
}
