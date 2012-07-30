using System;
using Com.Mobeelizer.Mobile.Wp7.Api;
using System.Xml.Serialization;

namespace Com.Mobeelizer.Mobile.Wp7
{
    internal class MobeelizerTombstoningState
    {
        [XmlElement]
        public bool LoggedIn { get; set; }

        [XmlElement]
        public String User { get; set; }

        [XmlElement]
        public String Instance { get; set; }

        [XmlElement]
        public String Password { get; set; }

        [XmlElement]
        public MobeelizerSyncStatus SyncStatus { get; set; }

        [XmlElement]
        public bool IsAllSynchronization { get; set; }

        [XmlElement]
        public String SyncTicket { get; set; }
    }
}
