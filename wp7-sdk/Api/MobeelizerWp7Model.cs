using System;

namespace Com.Mobeelizer.Mobile.Wp7.Api
{
    // TODO:
    public abstract class MobeelizerWp7Model
    {
        public virtual String guid { get; set; }

        protected String Owner
        {
            get
            {
                return null;
            }

            set
            {
            }
        }

        protected bool Conflicted
        {
            get
            {
                return false;
            }

            set
            {

            }
        }

        protected bool Modified
        {
            get
            {
                return false;
            }

            set
            {
                
            }
        }

        protected bool Deleted
        {
            get
            {
                return false;
            }

            set
            {

            }
        }
    }
}
