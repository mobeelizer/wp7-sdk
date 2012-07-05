﻿
namespace Com.Mobeelizer.Mobile.Wp7.Api
{
    public enum MobeelizerCredential
    {
        /// <summary>
        /// No permission. 
        /// </summary>
        NONE = 0,

        /// <summary>
        /// Permission only for own entities.
        /// </summary>
        OWN,

        /// <summary>
        ///  Permission only for group entities.
        /// </summary>
        GROUP,

        /// <summary>
        /// All permission. 
        /// </summary>
        ALL
    }
}
