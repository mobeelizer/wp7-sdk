﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Mobeelizer.Mobile.Wp7.Sync;
using System.Data.Linq;

namespace Com.Mobeelizer.Mobile.Wp7.Api
{
    public interface IMobeelizerDatabase
    {
        IMobeelizerTransaction BeginTransaction();
    }
}
