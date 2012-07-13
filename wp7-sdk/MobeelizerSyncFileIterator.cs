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

namespace Com.Mobeelizer.Mobile.Wp7
{
    public class MobeelizerSyncFileIterator
    {
        public bool HasNext { get; set; }

        internal string Next()
        {
            throw new NotImplementedException();
        }

        internal System.IO.Stream GetStream()
        {
            throw new NotImplementedException();
        }

        internal void Close()
        {
            //throw new NotImplementedException();
        }
    }
}
