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
using System.IO.IsolatedStorage;
using System.IO;

namespace Com.Mobeelizer.Mobile.Wp7.Others
{
    public class File
    {
        private String path;

        public File(String path)
        {
            this.path = path;
        }

        public void Create()
        {
            using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                try
                {
                    if (iso.FileExists(this.path))
                    {
                        iso.DeleteFile(this.path);
                    }

                    using (IsolatedStorageFileStream stream =iso.OpenFile(this.path, FileMode.CreateNew))
                    {
                        stream.Close();
                    }
                }
                catch (Exception e)
                {
                    throw new IOException(e.Message, e);
                }
            }
        }

        public IsolatedStorageFileStream OpenToRead()
        {
            IsolatedStorageFileStream stream = null;
            using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                try
                {
                    stream = iso.OpenFile(this.path, FileMode.Open, FileAccess.Read);
                }
                catch (Exception e)
                {
                    throw new IOException(e.Message, e);
                }
            }
            return stream;
        }

        public IsolatedStorageFileStream OpenToWrite()
        {
            IsolatedStorageFileStream stream = null;
            using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                try
                {
                    stream = iso.OpenFile(this.path, FileMode.Open, FileAccess.Write);
                }
                catch (Exception e)
                {
                    throw new IOException(e.Message, e);
                }
            }
            return stream;
        }

        public void Delete()
        {
            using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (iso.FileExists(this.path))
                {
                    try
                    {
                        iso.DeleteFile(this.path);
                    }
                    catch(Exception e)
                    {
                        throw new IOException(e.Message, e);
                    }
                }
            }
        }
    }
}
