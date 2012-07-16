using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;

namespace Com.Mobeelizer.Mobile.Wp7.Api
{
    public interface IMobeelizerTransaction : IDisposable
    {
        ITable<T> GetModels<T>() where T : MobeelizerWp7Model;

        void Commit();

        void Close();
    }
}
