using System;
using System.IO.IsolatedStorage;
using System.IO;
using Com.Mobeelizer.Mobile.Wp7.Sync;
using Com.Mobeelizer.Mobile.Wp7.Database;

namespace Com.Mobeelizer.Mobile.Wp7
{
    public class MobeelizerDataFileService
    {
        private const String TAG = "mobeelizer:datafileservice";

        private MobeelizerApplication application;

        public MobeelizerDataFileService(MobeelizerApplication application)
        {
            this.application = application;
        }

        internal bool PrepareOutputFile(Others.File outputFile)
        {
            MobeelizerOutputData outputData = null;
            MobeelizerSyncEnumerable enumerable = null;
            MobeelizerSyncFileEnumerable filesToSync = null;
            try
            {
                outputData = new MobeelizerOutputData(outputFile, GetTmpOutputFile());
                enumerable = application.GetDatabase().GetEntitiesToSync();
                foreach(MobeelizerJsonEntity entity in enumerable)
                {
                    Log.i(TAG, "Add entity to sync: " + entity.ToString());
                    outputData.WriteEntity(entity);
                }

                filesToSync = application.GetDatabase().GetFilesToSync();
                foreach(var file in filesToSync)
                {
                    String guid = file.Guid;
                    using (Stream stream = file.GetStream())
                    {
                        if (stream == null)
                        {
                            continue;
                        }

                        outputData.WriteFile(guid, stream);
                        Log.i(TAG, "Add file to sync: " + guid);
                    }
                }

                return true;
            }
            catch (IOException e)
            {
                throw new InvalidOperationException(e.Message, e);
            }
            finally
            {
                if (outputData != null)
                {
                    outputData.Close();
                }
            }
        }

        internal bool ProcessInputFile(Others.File inputFile, bool isAllSynchronization)
        {
            MobeelizerInputData inputData = null;
            try
            {
                inputData = new MobeelizerInputData(inputFile);
                application.GetFileService().AddFilesFromSync(inputData.GetFiles(), inputData);
                bool isSuccessful = ((MobeelizerDatabase)application.GetDatabase()).UpdateEntitiesFromSync(inputData.GetInputData(), isAllSynchronization);
                if (!isSuccessful)
                {
                    return false;
                }

                application.GetFileService().DeleteFilesFromSync(inputData.GetDeletedFiles());
                return true;
            }
            catch (IOException e)
            {
                throw new InvalidOperationException(e.Message, e);
            }
            finally
            {
                if (inputData != null)
                {
                    inputData.Close();
                }
            }
        }

        private Others.File GetTmpOutputFile()
        {
            String filePath = System.IO.Path.Combine("sync", "output");
            using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!iso.DirectoryExists("sync"))
                {
                    iso.CreateDirectory("sync");
                }
                if (iso.FileExists(filePath))
                {
                    iso.DeleteFile(filePath);
                }
            }

            Others.File file = new Others.File(filePath);
            file.Create();
            return file;
        }
    }
}
