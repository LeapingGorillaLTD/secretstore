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

using System.Collections.Generic;
using System.IO;
using Amazon.DynamoDBv2.Model;

namespace LeapingGorilla.SecretStore.Aws
{
	public static class AwsExtensions
	{
		/// <summary>
		/// Convert a DynamoDb GetItem response to a ProtectedSecret
		/// </summary>
		/// <param name="document"></param>
		/// <returns></returns>
		public static ProtectedSecret ToProtectedSecret(this Dictionary<string, AttributeValue> document)
		{
			return new ProtectedSecret
			{
				ApplicationName = document[AwsDynamoProtectedSecretRepository.Fields.ApplicationName].S,
				Name = document[AwsDynamoProtectedSecretRepository.Fields.SecretName].S,
				MasterKeyId = document[AwsDynamoProtectedSecretRepository.Fields.MasterKeyId].S,
				ProtectedDocumentKey = document[AwsDynamoProtectedSecretRepository.Fields.ProtectedDocumentKey].B.ToArray(),
				ProtectedSecretValue = document[AwsDynamoProtectedSecretRepository.Fields.ProtectedSecretValue].B.ToArray(),
				InitialisationVector = document[AwsDynamoProtectedSecretRepository.Fields.InitialisationVector].B.ToArray()
			};
		}
		
		/// <summary>
		/// Take a ProtectedSecret and convert it to a DynamoDb attribute map
		/// suitable for use in a PutItem request.
		/// </summary>
		/// <param name="secret">Secret we want to convert to an attribute map</param>
		/// <returns>Attribute map suitable for use in a DynamoDB PUT request</returns>
		public static Dictionary<string, AttributeValue> ToAttributeMap(this ProtectedSecret secret)
		{
			return new Dictionary<string, AttributeValue>
			{
				{AwsDynamoProtectedSecretRepository.Fields.ApplicationName, new AttributeValue {S = secret.ApplicationName}},
				{AwsDynamoProtectedSecretRepository.Fields.SecretName, new AttributeValue {S = secret.Name}},
				{AwsDynamoProtectedSecretRepository.Fields.MasterKeyId, new AttributeValue {S = secret.MasterKeyId}},
				{AwsDynamoProtectedSecretRepository.Fields.ProtectedDocumentKey, new AttributeValue {B = new MemoryStream(secret.ProtectedDocumentKey)}},
				{AwsDynamoProtectedSecretRepository.Fields.ProtectedSecretValue, new AttributeValue {B = new MemoryStream(secret.ProtectedSecretValue)}},
				{AwsDynamoProtectedSecretRepository.Fields.InitialisationVector, new AttributeValue {B = new MemoryStream(secret.InitialisationVector)}}
			};
		}
	}
}