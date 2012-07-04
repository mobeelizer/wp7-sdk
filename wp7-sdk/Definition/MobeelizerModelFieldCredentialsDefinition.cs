using System;
using Com.Mobeelizer.Mobile.Wp7.Api;

namespace Com.Mobeelizer.Mobile.Wp7.Definition
{
    class MobeelizerModelFieldCredentialsDefinition : IMobeelizerDefinition
    {
        internal String Role { get; set; }

        internal MobeelizerCredential ReadAllowed { get; set; }

        internal MobeelizerCredential CreateAllowed { get; set; }

        internal MobeelizerCredential UpdateAllowed { get; set; }

        public String DigestString
        {
            get
            {
                /// TODO: check this
                return this.Role
                        + "="
                        + String.Format("{0}{1}{2}{3}{4}", this.ReadAllowed, this.CreateAllowed, this.UpdateAllowed,
                                MobeelizerCredential.NONE, 0);
            }
        }
    }
}
