using System;
using Com.Mobeelizer.Mobile.Wp7.Api;

namespace Com.Mobeelizer.Mobile.Wp7.Definition
{
    public class MobeelizerModelCredentialsDefinition : IMobeelizerDefinition
    {
        public String Role { get; set; }

        public MobeelizerCredential ReadAllowed { get; set; }

        public MobeelizerCredential CreateAllowed { get; set; }

        public MobeelizerCredential UpdateAllowed { get; set; }

        public MobeelizerCredential DeleteAllowed { get; set; }

        public bool IsResolveConflictAllowed { get; set; }

        public String DigestString
        {
            get
            {
                /// TODO: check this
                return this.Role
                        + "="
                        + String.Format("{0}{1}{2}{3}{4}", this.ReadAllowed, this.CreateAllowed, this.UpdateAllowed,
                        this.DeleteAllowed, IsResolveConflictAllowed ? 1 : 0);
            }
        }
    }
}