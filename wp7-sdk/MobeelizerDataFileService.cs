using System;
using System.IO.IsolatedStorage;
using System.IO;
using Com.Mobeelizer.Mobile.Wp7.Sync;

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
            MobeelizerSyncFileIterator fileIterator = null;
            try
            {
                outputData = new MobeelizerOutputData(outputFile, GetTmpOutputFile());
                enumerable = application.GetDatabase().GetEntitiesToSync();
                foreach(MobeelizerJsonEntity entity in enumerable)
                {
                    Log.i(TAG, "Add entity to sync: " + entity.ToString());
                    outputData.WriteEntity(entity);
                }

                fileIterator = application.GetDatabase().GetFilesToSync();
                while (fileIterator.HasNext)
                {
                    String guid = fileIterator.Next();
                    Stream stream = fileIterator.GetStream();

                    if (stream == null)
                    {
                        continue; // TODO V3 external storage was removed?
                    }

                    outputData.WriteFile(guid, stream);
                    Log.i(TAG, "Add file to sync: " + guid);
                }

                return true;
            }
            catch (IOException e)
            {
                throw new InvalidOperationException(e.Message, e);
            }
            finally
            {
                if (enumerable != null)
                {
                    enumerable.GetEnumerator().Dispose();
                }
                if (fileIterator != null)
                {
                    fileIterator.Close();
                }
                if (outputData != null)
                {
                    outputData.Close();
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
                if(iso.FileExists(filePath))
                {
                    iso.DeleteFile(filePath);
                }
            }

            Others.File file = new Others.File(filePath);
            file.Create();
            return file;
        }

        internal bool ProcessInputFile(Others.File inputFile, bool isAllSynchronization)
        {
            MobeelizerInputData inputData = null;
            try
            {
                inputData = new MobeelizerInputData(inputFile);
                application.GetFileService().AddFilesFromSync(inputData.GetFiles(), inputData);
                bool isSuccessful = application.GetDatabase().UpdateEntitiesFromSync(inputData.GetInputData(), isAllSynchronization);
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
    }
}
