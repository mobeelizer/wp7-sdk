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
using System.IO;

namespace wp7_sdk_unitTests.Helpers.Mock
{
    public class UTWebResponse : WebResponse
    {
        private Stream stream;

        public UTWebResponse(Stream stream)
        {
            this.stream = stream;
        }

        public override System.IO.Stream GetResponseStream()
        {
            return this.stream;
        }
    }
}
