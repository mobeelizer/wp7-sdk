using System;
using System.Diagnostics;

namespace Com.Mobeelizer.Mobile.Wp7
{
    internal class Log
    {
        internal static void i(String tag, String message, String version = null)
        {
            if (version == null)
            {
                Debug.WriteLine("{0}\t{1}", tag, message);
                Debug.WriteLine("{0}\t{1}", tag, message);
            }
            else
            {
                Debug.WriteLine("{0}\t{1}\tv{2}", tag, message, version);
                Debug.WriteLine("{0}\t{1}", tag, message);
            }
        }
    }
}
