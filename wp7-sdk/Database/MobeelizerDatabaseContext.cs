﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Data.Linq;
using Com.Mobeelizer.Mobile.Wp7.Api;

namespace Com.Mobeelizer.Mobile.Wp7.Database
{
    internal class MobeelizerDatabaseContext : DataContext
    {
        internal MobeelizerDatabaseContext(String connectinString) :
            base(connectinString)
        {
        }

        internal MobeelizerDatabaseContext(String instanceGuid, String user) :
            base(String.Format("DataSource=isostore:/{0}_{1}_data.sdf", instanceGuid, user))
        {
        }

        internal ITable<T> GetModels<T>() where T: MobeelizerWp7Model
        {
            return new MobeelizerTable<T>(this.GetTable<T>(), this);
        }

        internal Table<MobeelizerModelMetadata> ModelMetadata
        {
            get
            {
                return GetTable<MobeelizerModelMetadata>();
            }
        }

        internal Table<MobeelizerFilesTableEntity> Files
        {
            get
            {
                return GetTable<MobeelizerFilesTableEntity>();
            }
        }
    }
}
