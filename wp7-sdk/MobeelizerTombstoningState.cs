using System;
using Com.Mobeelizer.Mobile.Wp7.Api;
using System.Xml.Serialization;

namespace Com.Mobeelizer.Mobile.Wp7
{
    /// <summary>
    /// Data saved on tombstoning process.
    /// </summary>
    public class MobeelizerTombstoningState
    {
        /// <summary>
        /// Indicates whether user is logged in.
        /// </summary>
        [XmlElement]
        public bool LoggedIn { get; set; }

        /// <summary>
        /// User name.
        /// </summary>
        [XmlElement]
        public String User { get; set; }

        /// <summary>
        /// Instance name.
        /// </summary>
        [XmlElement]
        public String Instance { get; set; }

        /// <summary>
        /// User password.
        /// </summary>
        [XmlElement]
        public String Password { get; set; }

        /// <summary>
        /// Synchronization status.
        /// </summary>
        [XmlElement]
        public MobeelizerSyncStatus SyncStatus { get; set; }

        /// <summary>
        /// Indicates wheter it was all synchronization.
        /// </summary>
        [XmlElement]
        public bool IsAllSynchronization { get; set; }

        /// <summary>
        /// Synchronization ticket.
        /// </summary>
        [XmlElement]
        public String SyncTicket { get; set; }
    }
}
