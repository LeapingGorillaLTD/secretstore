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

using CommandLine;

namespace LeapingGorilla.SecretStore.CommandLine.Options
{
	[Verb("set", HelpText = "Add or update a secret in a Secret Store")]
	public class SetSecretOptions
	{
		[Option('t', "tableName", Required = true,
			HelpText = "Name of the Dynamo table containing the secret")]
		public string TableName { get; set; }
		
		[Option('a', "applicationName", Required = true,
			HelpText = "Name of the application for the secret (like YourApp.Api)")]
		public string ApplicationName { get; set; }

		[Option('s', "secretName", Required = true,
			HelpText = "Name of the secret")]
		public string SecretName { get; set; }
		
		[Option('k', "key", Required = true,
			HelpText = "ID of the key used to protect the secret. This should be the ARN of a KMS key or its alias in the format 'alias/YourKeyName'")]
		public string Key { get; set; }
	}
}
