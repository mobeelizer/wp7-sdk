using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.IsolatedStorage;

namespace Com.Mobeelizer.Mobile.Wp7.Api
{
    public interface IMobeelizerFile
    {
        String Guid { get; }

        String Name { get; }

        IsolatedStorageFileStream GetStream();
    }
}
