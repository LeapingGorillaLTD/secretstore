using System;
using System.Runtime.InteropServices;
using System.Security;

namespace LeapingGorilla.SecretStore.CommandLine
{
    public static class StringExtensions
    {
	    public static string ToUnprotectedString(this SecureString value)
	    {
			IntPtr valuePtr = IntPtr.Zero;
		    try
		    {
			    valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
				return Marshal.PtrToStringUni(valuePtr);
		    }
		    finally
		    {
			    Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
		    }
	    }
	}
}
