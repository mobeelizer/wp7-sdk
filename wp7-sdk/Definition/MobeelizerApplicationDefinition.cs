using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Com.Mobeelizer.Mobile.Wp7.Definition
{
    public class MobeelizerApplicationDefinition : IMobeelizerDefinition
    {
        private String digest;

        public String Vendor { get; set; }

        public String Application { get; set; }

        public String DigestString
        {
            get
            {
                if (this.digest == null)
                {
                    StringBuilder sb = new StringBuilder();

                    sb.Append(this.Vendor).Append("$").Append(this.Application).Append("$").Append(this.ConflictMode).Append("$");

                    MobeelizerApplicationDefinition.DigestSortJoinAndAdd(sb, this.Devices);

                    sb.Append("$");

                    MobeelizerApplicationDefinition.DigestSortJoinAndAdd(sb, this.Groups);

                    sb.Append("$");

                    MobeelizerApplicationDefinition.DigestSortJoinAndAdd(sb, this.Roles);

                    sb.Append("$");

                    MobeelizerApplicationDefinition.DigestSortJoinAndAdd(sb, this.Models);

                    digest = Encrypt(sb.ToString());
                }

                return this.digest;
            }
        }

        internal String ConflictMode { get; set; }

        internal IList<MobeelizerDeviceDefinition> Devices { get; set; }

        internal IList<MobeelizerGroupDefinition> Groups { get; set; }

        internal IList<MobeelizerRoleDefinition> Roles { get; set; }

        internal IList<MobeelizerModelDefinition> Models { get; set; }

        internal static void DigestSortJoinAndAdd<T>(StringBuilder sb, IList<T> collection) where T : IMobeelizerDefinition
        {
            if (collection == null || collection.Count == 0)
            {
                return;
            }

            List<String> list = new List<String>();
            foreach (T item in collection)
            {
                list.Add(item.DigestString);
            }

            list.Sort();
            bool first = true;
            foreach (String item in list)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append("&");
                }

                sb.Append(item);
            }
        }

        private string Encrypt(String plainText)
        {
            // TODO: check this
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            HashAlgorithm hash = new SHA256Managed();
            byte[] hashBytes = hash.ComputeHash(plainTextBytes);
            String hashValue = Convert.ToBase64String(hashBytes);
            return hashValue;
        }
    }
}
