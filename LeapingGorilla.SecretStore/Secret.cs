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
