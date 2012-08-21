using System;
using System.IO.IsolatedStorage;
using Com.Mobeelizer.Mobile.Wp7.Api;
using Newtonsoft.Json.Linq;

namespace Com.Mobeelizer.Mobile.Wp7
{
    internal class MobeelizerFile : IMobeelizerFile
    {
        public MobeelizerFile(string name, System.IO.Stream stream)
        {
            this.Name = name;
            this.Guid = Mobeelizer.Instance.GetFileService().AddFile(stream);
        }

        public MobeelizerFile(string name, string guid)
        {
            this.Name = name;
            this.Guid = guid;
        }

        public MobeelizerFile(string json)
        {
            JObject jobject = JObject.Parse(json);
            this.Name = (String)jobject["filename"];
            this.Guid = (String)jobject["guid"];
        }

        public string Guid { get; private set; }

        public string Name { get; private set; }

        public IsolatedStorageFileStream GetStream()
        {
            return Mobeelizer.Instance.GetFileService().GetFile(Guid);
        }

        internal string GetJson()
        {
            JObject jobject = new JObject();
            jobject.Add("filename", this.Name);
            jobject.Add("guid", this.Guid);
            return jobject.ToString();
        }


        public Uri Uri
        {
            get 
            {
                return Mobeelizer.Instance.GetFileService().GetFileUri(Guid);
            }
        }
    }
}
