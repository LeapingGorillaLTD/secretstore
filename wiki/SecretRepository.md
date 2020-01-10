Secret Repositories store the protected values of secrets. Each secret is encrypted by it's own document key. Each document key is encrypted with an overarching master key. Multiple secret repository implementations are provided so you can find a store that fits your needs. Alternatively you may implement the `IProtectedSecretRepository` and provide your own secret repository.

## Implementations ##

 * [`AwsDynamoProtectedSecretRepository`](AwsDynamoProtectedSecretRepository)