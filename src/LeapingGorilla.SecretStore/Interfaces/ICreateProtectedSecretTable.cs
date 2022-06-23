using System.Threading.Tasks;

namespace LeapingGorilla.SecretStore.Interfaces
{
	///<summary>The implementing class knows how to make a protected secret table</summary>
	public interface ICreateProtectedSecretTable
	{
		/// <summary>
		/// Creates a table in the data source suitable for storing protected secrets.
		/// The table will be created asynchronously. 
		/// </summary>
		/// <param name="secretTableName">
		/// The name we should use for the table containing the secrets
		/// </param>
		Task CreateProtectedSecretTableAsync(string secretTableName);
	}
}