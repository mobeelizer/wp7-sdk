using System;
using System.Linq;
using Com.Mobeelizer.Mobile.Wp7.Database;

namespace Com.Mobeelizer.Mobile.Wp7.Api
{
    /// <summary>
    /// Models base class.
    /// </summary>
    public abstract class MobeelizerWp7Model
    {
        /// <summary>
        /// Model guid.
        /// </summary>
        /// <value>Unique model identificatior.</value>
        public virtual String Guid { get; set; }

        /// <summary>
        /// Model owner. 
        /// </summary>
        /// <value>Owner of current model.</value>
        public virtual String Owner { get; set; }

        /// <summary>
        /// Conflicted flag.
        /// </summary>
        /// <value>True if model is in conflict.</value>
        public virtual bool Conflicted { get; set; }

        /// <summary>
        /// Modification flag.
        /// </summary>
        /// <value>True if model has been modified since last sync. </value>
        public virtual bool Modified { get; set; }
        
        /// <summary>
        /// Deleted flag.
        /// </summary>
        /// <value> True if model has been deleted since last sync.</value>
        public virtual bool Deleted { get; set; }
        
        /// <summary>
        /// Opens file from json entity.
        /// </summary>
        /// <param name="file">Json entity string value.</param>
        /// <returns>File instance.</returns>
        protected IMobeelizerFile GetFile(string file)
        {
            try
            {
                return new MobeelizerFile(file);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Generates json entity from file instance.
        /// </summary>
        /// <param name="value">File instance.</param>
        /// <returns>Json entity string value.</returns>
        protected String SetFile(IMobeelizerFile value)
        {
            return (value as MobeelizerFile).GetJson();
        }
    }
}
