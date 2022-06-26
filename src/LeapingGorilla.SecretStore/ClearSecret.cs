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
	///<summary>An unprotected secret</summary>
	public class ClearSecret : Secret
	{
		///<summary></summary>
		public string Value { get; set; }
		

		public ClearSecret(string applicationName, string secretName, string secretValue)
			:base(applicationName, secretName)
		{
			Value = secretValue;
		}
	}
}
