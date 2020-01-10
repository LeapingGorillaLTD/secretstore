Models a single secret that is protected within the Secret Store. The secret is protected with a document key. The document key itself is protected by a master key.

Field                        | Purpose
-----------------------------|----------
ApplicationName              | Name of the application the secret belongs to (i.e. YourApp)
Name                         | Name of the secret (e.g. "Main DB Connection String)
MasterKeyId                  | ID of the master key used to protect the document key
ProtectedDocumentKey         | Document Key used to protect the value of this secret. This is encrypted with the master key identified in KeyId
ProtectedSecretValue         | Value of the secret encrypted with the document key and initialisation vector
InitialisationVector         | IV used alongside the document key to