using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Com.Mobeelizer.Mobile.Wp7.Definition
{
    internal class MobeelizerApplicationDefinition : IMobeelizerDefinition
    {
        internal String digest;

        internal String Vendor { get; set; }

        internal String Application { get; set; }

        public String Digest
        {
            get
            {
                if (this.digest == null)
                {   
                    this.digest = Encrypt(this.DigestString);                
                }

                return this.digest;
            }
        }

        public String DigestString
        {
            get
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
                return sb.ToString();
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
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            HashAlgorithm hash = new SHA256Managed();
            byte[] hashBytes = hash.ComputeHash(plainTextBytes);
            String hashValue = ByteArrayToHexString(hashBytes);
            return hashValue;
        }

        private String ByteArrayToHexString(byte[] b)
        {
            StringBuilder result = new StringBuilder();
            foreach (byte element in b)
            {
                result.Append(((element & 0xff) + 0x100).ToString("X").Substring(1).ToLower());
            }

            return result.ToString();
        }
    }
}
