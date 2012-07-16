using System;
using System.IO.IsolatedStorage;
using System.IO;

namespace Com.Mobeelizer.Mobile.Wp7.Others
{
    internal class File
    {
        private String path;

        internal File(String path)
        {
            this.path = path;
        }

        internal void Create()
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

        internal IsolatedStorageFileStream OpenToRead()
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

        internal IsolatedStorageFileStream OpenToWrite()
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

        internal void Delete()
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
