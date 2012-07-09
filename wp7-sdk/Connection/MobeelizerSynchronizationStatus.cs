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

namespace Com.Mobeelizer.Mobile.Wp7.Connection
{
    internal enum MobeelizerSynchronizationStatus
    {
        WAITING,
        PENDING, //- task is being currently processing
        FINISHED, // - task has been finished with success
        REJECTED,// - task has been finished with failure
        CONFIRMED //- task has been already confirmed
    }
}
