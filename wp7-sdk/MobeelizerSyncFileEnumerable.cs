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
using System.Collections;
using System.Collections.Generic;
using Com.Mobeelizer.Mobile.Wp7.Database;
using System.Linq;
using Com.Mobeelizer.Mobile.Wp7.Api;

namespace Com.Mobeelizer.Mobile.Wp7
{
    internal class MobeelizerSyncFileEnumerable : IEnumerable<IMobeelizerFile>
    {
        private Database.MobeelizerDatabaseContext transaction;

        private MobeelizerSyncFileEnumerator enumerator;

        internal MobeelizerSyncFileEnumerable(Database.MobeelizerDatabaseContext transaction)
        {
            this.transaction = transaction;
            this.enumerator = new MobeelizerSyncFileEnumerator(transaction);
        }

        public IEnumerator<IMobeelizerFile> GetEnumerator()
        {
            return this.enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    internal class MobeelizerSyncFileEnumerator : IEnumerator<IMobeelizerFile>
    {
        private Database.MobeelizerDatabaseContext transaction;

        private IEnumerator<MobeelizerFilesTableEntity> enumerator;

        public MobeelizerSyncFileEnumerator(Database.MobeelizerDatabaseContext transaction)
        {
            this.transaction = transaction;
            enumerator = (from f in transaction.Files where f.Modyfied == 2 select f).GetEnumerator();
        }

        public IMobeelizerFile Current
        {
            get 
            {
                return new MobeelizerFile(System.IO.Path.GetFileName(this.enumerator.Current.Path), this.enumerator.Current.Guid);
            }
        }

        public void Dispose()
        {
            this.enumerator.Dispose();
            this.transaction.Dispose();
        }

        object IEnumerator.Current
        {
            get { throw new NotImplementedException(); }
        }

        public bool MoveNext()
        {
            return this.enumerator.MoveNext();
        }

        public void Reset()
        {
            this.enumerator.Reset();
        }
    }
}
