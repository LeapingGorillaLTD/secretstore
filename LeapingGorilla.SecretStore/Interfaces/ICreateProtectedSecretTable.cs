using System.Threading.Tasks;

namespace LeapingGorilla.SecretStore.Interfaces
{
	///<summary>The implementing class knows how to make a protected secret table</summary>
	public interface ICreateProtectedSecretTable
	{
		/// <summary>
		/// Creates the protected secret table asynchronously.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		/// <returns>Task to await</returns>
		Task CreateProtectedSecretTableAsync(string tableName);
	}
}
