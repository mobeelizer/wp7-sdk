using System;
using System.Linq;
using Com.Mobeelizer.Mobile.Wp7.Database;

namespace Com.Mobeelizer.Mobile.Wp7.Api
{
    public abstract class MobeelizerWp7Model
    {
        private MobeelizerModelMetadata metadata;

        public virtual String guid { get; set; }

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
                if (Metadata != null)
                {
                    this.Metadata.Owner = value;
                    this.UpdateMetadata();
                }
            }
        }

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
                if (Metadata != null)
                {
                    this.Metadata.Conflicted = value ? 1 : 0;
                    this.UpdateMetadata();
                }
            }
        }

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
                if (Metadata != null)
                {
                    this.Metadata.Modyfied = value ? 1 : 0;
                    this.UpdateMetadata();
                }
            }
        }

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
                if (Metadata != null)
                {
                    this.Metadata.Deleted = value ? 1 : 0;
                    this.UpdateMetadata();
                }
            }
        }

        protected IMobeelizerFile GetFile(string picture)
        {
            try
            {
                return new MobeelizerFile(picture);
            }
            catch
            {
                return null;
            }
        }

        protected String SetFile(IMobeelizerFile value)
        {
            return (value as MobeelizerFile).GetJson();
        }

        private MobeelizerModelMetadata Metadata
        {
            get
            {
                if (this.metadata == null)
                {
                    try
                    {
                        using (var transaction = Mobeelizer.Instance.GetDatabase().BeginInternalTransaction())
                        {
                            var query = from m in transaction.ModelMetadata where m.Guid == this.guid select m;
                            this.metadata = query.Single();
                        }
                    }
                    catch { }
                }

                return this.metadata;
            }
        }

        private void UpdateMetadata()
        {
            using (var transaction = Mobeelizer.Instance.GetDatabase().BeginInternalTransaction())
            {
                var query = from m in transaction.ModelMetadata where m.Guid == this.guid select m;
                MobeelizerModelMetadata oldmetadata = query.Single();
                oldmetadata.Deleted = metadata.Deleted;
                oldmetadata.Owner = metadata.Owner;
                oldmetadata.Modyfied = metadata.Modyfied;
                oldmetadata.Conflicted = metadata.Conflicted;
                transaction.SubmitChanges();
            }
        }
    }
}
