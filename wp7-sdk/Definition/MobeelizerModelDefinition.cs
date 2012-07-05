using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Mobeelizer.Mobile.Wp7.Definition
{
    public class MobeelizerModelDefinition : IMobeelizerDefinition
    {
        public String Name { get; set; }

        public IList<MobeelizerModelFieldDefinition> Fields { get; set; }

        public IList<MobeelizerModelCredentialsDefinition> Credentials { get; set; }

        public String DigestString
        {
            get
            {
                StringBuilder sb = new StringBuilder().Append(this.Name).Append("{");
                MobeelizerApplicationDefinition.DigestSortJoinAndAdd(sb, this.Fields);
                sb.Append("$");
                MobeelizerApplicationDefinition.DigestSortJoinAndAdd(sb, this.Credentials);
                sb.Append("}");
                return sb.ToString();
            }
        }
    }
}
