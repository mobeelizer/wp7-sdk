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

namespace wp7_sdk_unitTests.Helpers.Mock
{
    public class UTWebRequestCreate : IWebRequestCreate
    {
        public WebRequest Create(Uri uri)
        {
            UTWebRequest request = new UTWebRequest(uri);
            return request;
        }
    }
}
