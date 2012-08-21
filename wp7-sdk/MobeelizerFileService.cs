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
using System.IO;
using System.IO.IsolatedStorage;

namespace Com.Mobeelizer.Mobile.Wp7
{
    internal class MobeelizerFileService
    {
        private const String TAG = "mobeelizer:fileservice";

        private MobeelizerApplication application;

        internal MobeelizerFileService(MobeelizerApplication application)
        {
            this.application = application;
        }

        internal String AddFile(Stream stream)
        {
            String guid = Guid.NewGuid().ToString();
            String path = SavaFile(guid, stream);
            application.GetDatabase().AddFile(guid, path);
            return guid;
        }

        private string SavaFile(string guid, Stream stream)
        {
            IsolatedStorageFileStream file = GetFile(guid);
            try
            {
                stream.CopyTo(file);
                file.Flush();
            }
            catch (FileNotFoundException e)
            {
                throw new InvalidOperationException(e.Message, e);
            }
            catch (IOException e)
            {
                throw new InvalidOperationException(e.Message, e);
            }
            finally
            {
                if (file != null)
                {
                    try
                    {
                        file.Close();
                    }
                    catch (IOException e)
                    {
                        Log.i(TAG, e.Message);
                    }
                }
            }

            return PreparePath(guid);
        }

        internal void AddFilesFromSync(IList<string> files, Sync.MobeelizerInputData inputData)
        {
            foreach (String guid in files)
            {
                if (application.GetDatabase().IsFileExists(guid))
                {
                    Log.i(TAG, "Skip existing file from sync: " + guid);
                    continue;
                }

                Log.i(TAG, "Add file from sync: " + guid);
                String path = null;
                try
                {
                    path = SavaFile(guid, inputData.GetFile(guid));
                }
                catch (IOException e)
                {
                    Log.i(TAG, e.Message);
                    path = "/unknown";
                }

                application.GetDatabase().AddFileFromSync(guid, path);
            }
        }

        internal void DeleteFilesFromSync(IList<string> files)
        {
            foreach (String guid in files)
            {
                Log.i(TAG, "Delete file from sync: " + guid);
                String path = application.GetDatabase().GetFilePath(guid);
                if (path == null)
                {
                    continue;
                }

                using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        iso.DeleteFile(path);
                    }
                    catch (IsolatedStorageException)
                    {
                        Log.i(TAG, "Cannot remove file " + path);
                    }
                }

                application.GetDatabase().DeleteFileFromSync(guid);
            }
        }

        private IsolatedStorageFile GetStorageDirectory()
        {
            IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication();
            String path = "mobeelizer";
            if (!iso.DirectoryExists(path))
            {
                iso.CreateDirectory(path);
            }

            path = System.IO.Path.Combine(path, application.Application);
            if (!iso.DirectoryExists(path))
            {
                iso.CreateDirectory(path);
            }

            path = System.IO.Path.Combine(path, application.Instance);
            if (!iso.DirectoryExists(path))
            {
                iso.CreateDirectory(path);
            }

            path = System.IO.Path.Combine(path, application.User);
            if (!iso.DirectoryExists(path))
            {
                iso.CreateDirectory(path);
            }

            return iso;
        }

        internal IsolatedStorageFileStream GetFile(String guid)
        {
            IsolatedStorageFileStream stream = null;
            using (IsolatedStorageFile iso = GetStorageDirectory())
            {
                stream = iso.OpenFile(this.PreparePath(guid), FileMode.OpenOrCreate);
            }

            return stream;
        }

        private String PreparePath(String guid)
        {
            String path = "mobeelizer";
            path = System.IO.Path.Combine(path, application.Application);
            path = System.IO.Path.Combine(path, application.Instance);
            path = System.IO.Path.Combine(path, application.User);
            path = System.IO.Path.Combine(path, guid);
            return path;
        }

        internal Uri GetFileUri(string guid)
        {
            return new Uri(this.PreparePath(guid), UriKind.Relative);
        }
    }
}
