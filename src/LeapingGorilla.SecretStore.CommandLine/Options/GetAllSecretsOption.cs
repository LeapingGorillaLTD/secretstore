﻿// /*
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
	[Verb("getAll", HelpText = "Get all secrets for an application from the Secret Store")]
	public class GetAllSecretsOption
	{
		[Option('t', "tableName", Required = true,
			HelpText = "Name of the Dynamo table that we are reading the secrets from")]
		public string TableName { get; set; }
		
		[Option('a', "applicationName", Required = true,
			HelpText = "Name of the application for the secrets (like YourApp.Api)")]
		public string ApplicationName { get; set; }
	}
}
