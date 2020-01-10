# Secret Store

Secret Store is a platform agnostic method of storing and distributing your secrets. This project provides libraries giving you an API to code against with interchangeable implementations for how your secrets are persisted. Support is provided out of the box for Public/Private (RSA) protection of secrets using your own data storage implementation and for secrets stored in [AWS Dynamo](https://aws.amazon.com/dynamodb/) secured using [AWS KMS](https://aws.amazon.com/kms/).

# What Problem are we Solving?

Storing secrets is hard. Connection strings, passwords, sensitive URL's, API tokens, the list goes on and on. You need a way to store these secrets in a central location (so updating the secret once makes the new value available to all of your deployed code). You need a way to secure access to those secrets for a couple of cases - first in the event of a data breach the secrets plain text should not be exposed and second you should be able to isolate your secrets so programs can view only the secrets that they need access to. 

# How do we solve it?

We provide a reference implementation of the Secret Store API's that store your secrets using AWS. The secrets are stored in Dynamo (a resilient, single location) with their sensitive content encrypted using KMS. This gives you a number of unique abilities: you can restrict access to the dynamo table at the role level for your code (whether that is via access key or IAM Role) [down to the Partition Key](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/specifying-conditions.html) which in our implementation is the Application Name. You can apply a second layer of control to this by restricting access to the KMS keys that protect the secret and you can use as many different KMS keys as you like (1 per secret). We recommend that you use a unique key per application per environment (i.e. YourApp.Dev, YourApp.Production, YourOtherApp.Dev, YourOtherApp.Production) and a different Dynamo Table per environment (i.e. Dev/Production) to reduce the potential impact of any breach or misconfiguration.

# How are secrets stored?

In the reference implementation Secrets are stored as documents in DynamoDB. Each Secret stores the Application Name, the secret name, an initialisation vector (IV), the master key that was used to protect the key for the secret, a document key which was used to protect the secret itself and the encrypted secret value. 

We apply a key hierarchy of Master Key -> Document Key -> Secret for a few reasons. First it provides segmentation - each secret has its own key. If any one key is leaked or breached it can only expose a single secret. Second it minimises the amount of plaintext that needs to be protected. Only the master key needs to be secured as all of the document keys are encrypted. Using KMS the master key lives in a [HSM](https://en.wikipedia.org/wiki/Hardware_security_module) further reducing the surface area of attack. Finally it stops the need to perform bulk encryption in the HSM itself. KMS imposes a physical limitation of the amount of data that it can protect - [4KB of data](https://docs.aws.amazon.com/kms/latest/APIReference/API_Encrypt.html) so if your secret is larger than that we would need to use this pattern anyway.

# How are secrets protected?

A document key is generated using KMS. This is a random array of bytes created using a HSM. An initialisation vector (IV) is also created - another random aray of bytes. Your secret is encrypted using the document key with AES-128-CBC with PKCS7 padding using the random IV. The document key is encrypted using the master key. The encrypted secret, ID of the master key and the IV are all stored in the Dynamo table.

# Why the emphasis on AWS?

It's what we use in our day to day lives. It fits well in our infrastructure and we understand the performance/security trade offs. Secret Store itself provides interfaces at all levels so you can inject custom implementations for any aspect of your secret workflow to add auditing etc. If you're interested in giving back feel free to open a PR with any improvements.

# How is it licensed?

Secret Store is Open Source made available using the [Apache v2 license](https://www.apache.org/licenses/LICENSE-2.0). 

[View all content](https://bitbucket.org/LeapingGorillaLTD/secretstore/wiki/browse/)