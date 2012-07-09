using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
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
            }
            else
            {
                Debug.WriteLine("{0}\t{1}\tv{2}", tag, message, version);
            }
        }
    }
}
