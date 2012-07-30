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
        public virtual String guid { get; set; }

        /// <summary>
        /// Model owner. 
        /// </summary>
        /// <value>Owner of current model.</value>
        protected virtual String owner
        {
            get
            {
                if (this.Metadata != null)
                {
                    return this.Metadata.Owner;
                }
                else
                {
                    return null;
                }
            }

            set
            {

                try
                {
                    using (var transaction = Mobeelizer.Instance.GetDatabase().BeginInternalTransaction())
                    {
                        var query = from m in transaction.ModelMetadata where m.Guid == this.guid select m;
                        MobeelizerModelMetadata oldmetadata = query.Single();
                        oldmetadata.Owner = value;
                        transaction.SubmitChanges();
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Conflicted flag.
        /// </summary>
        /// <value>True if model is in conflict.</value>
        protected bool conflicted
        {
            get
            {
                if (this.Metadata != null)
                {
                    return this.Metadata.Conflicted == 1;
                }
                else
                {
                    return false;
                }
            }

            set
            {

                try
                {
                    using (var transaction = Mobeelizer.Instance.GetDatabase().BeginInternalTransaction())
                    {
                        var query = from m in transaction.ModelMetadata where m.Guid == this.guid select m;
                        MobeelizerModelMetadata oldmetadata = query.Single();
                        oldmetadata.Conflicted = value ? 1 : 0;
                        transaction.SubmitChanges();
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Modification flag.
        /// </summary>
        /// <value>True if model has been modified since last sync. </value>
        protected bool modified
        {
            get
            {
                if (this.Metadata != null)
                {
                    return this.Metadata.Modyfied != 0;
                }
                else
                {
                    return false;
                }
            }

            set
            {
                try
                {
                    using (var transaction = Mobeelizer.Instance.GetDatabase().BeginInternalTransaction())
                    {
                        var query = from m in transaction.ModelMetadata where m.Guid == this.guid select m;
                        MobeelizerModelMetadata oldmetadata = query.Single();
                        oldmetadata.Modyfied = value ? 1 : 0;
                        transaction.SubmitChanges();
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Deleted flag.
        /// </summary>
        /// <value> True if model has been deleted since last sync.</value>
        protected bool deleted
        {
            get
            {
                if (this.Metadata != null)
                {
                    return this.Metadata.Deleted != 0;
                }
                else
                {
                    return false;
                }
            }

            set
            {
                try
                {
                    using (var transaction = Mobeelizer.Instance.GetDatabase().BeginInternalTransaction())
                    {
                        var query = from m in transaction.ModelMetadata where m.Guid == this.guid select m;
                        MobeelizerModelMetadata oldmetadata = query.Single();
                        oldmetadata.Deleted = value ? 1 : 0;
                        transaction.SubmitChanges();
                    }
                }
                catch { }
            }
        }

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

        private MobeelizerModelMetadata Metadata
        {
            get
            {
                MobeelizerModelMetadata metadata = null;
                try
                {
                    using (var transaction = Mobeelizer.Instance.GetDatabase().BeginInternalTransaction())
                    {
                        var query = from m in transaction.ModelMetadata where m.Guid == this.guid select m;
                        metadata = query.Single();
                    }
                }
                catch { }

                return metadata;
            }
        }
    }
}
