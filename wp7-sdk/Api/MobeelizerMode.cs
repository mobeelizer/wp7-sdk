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

namespace Com.Mobeelizer.Mobile.Wp7.Api
{
    public enum MobeelizerMode
    {
        /// <summary>
        /// The connections won't be performed. 
        /// </summary>
        DEVELOPMENT,

        /// <summary>
        /// The connections will be established to test instances.
        /// </summary>
        TEST,

        /// <summary>
        /// The connections will be established to production instances.
        /// </summary> 
        PRODUCTION
    }
}
