using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeapingGorilla.SecretStore
{
	public class RsaKeyInfo
	{
		public string KeyName { get; set; }

		public byte[] Signature { get; set; }
	}
}
