using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Mobeelizer.Mobile.Wp7.Definition
{
    public class MobeelizerModelFieldDefinition : IMobeelizerDefinition
    {
        internal String Name { get; set; }

        internal MobeelizerFieldType Type { get; set; }

        internal IList<MobeelizerModelFieldCredentialsDefinition> Credentials { get; set; }

        internal bool IsRequired { get; set; }

        internal String DefaultValue { get; set; }

        internal Dictionary<String, String> Options { get; set; }

        public string DigestString
        {
            get
            {
                StringBuilder sb = new StringBuilder().Append(Name).Append("{");
                sb.Append(Type.ToString()).Append("$");
                sb.Append(IsRequired).Append("$");
                sb.Append(DefaultValue).Append("$");
                MobeelizerApplicationDefinition.DigestSortJoinAndAdd(sb, Credentials);
                sb.Append("$");
                if (Options != null)
                {
                    List<String> optionsList = new List<String>();
                    foreach (KeyValuePair<String, String> option in Options)
                    {
                        optionsList.Add(option.Key + "=" + option.Value);
                    }

                    optionsList.Sort();
                    bool first = true;
                    foreach (String optionString in optionsList)
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            sb.Append("&");
                        }

                        sb.Append(optionString);
                    }
                }

                sb.Append("}");
                return sb.ToString();
            }
        }
    }
}
