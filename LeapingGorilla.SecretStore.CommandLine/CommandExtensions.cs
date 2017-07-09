using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.CommandLineUtils;

namespace LeapingGorilla.SecretStore.CommandLine
{
    public static class CommandExtensions
    {
	    public static bool HasValue(this CommandArgument arg)
	    {
		    return !String.IsNullOrWhiteSpace(arg?.Value);
	    }

	    public static string ValueOrDefault(this CommandArgument arg, string defaultValue = null)
	    {
		    return arg.HasValue() ? arg.Value : defaultValue;
	    }
    }
}
