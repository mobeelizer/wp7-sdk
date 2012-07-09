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
using Com.Mobeelizer.Mobile.Wp7.Api;
using System.IO;
using System.IO.IsolatedStorage;
using Com.Mobeelizer.Mobile.Wp7.Connection;

namespace Com.Mobeelizer.Mobile.Wp7
{
    public class MobeelizerSyncServicePerformer
    {
        private const String TAG = "mobeelizer:syncserviceperformer";
        private MobeelizerApplication application;
        private bool isAllSynchronization;
        private MobeelizerDataFileService dataFileService;

        public MobeelizerSyncServicePerformer(MobeelizerApplication application, bool syncAll)
        {
            this.application = application;
            this.isAllSynchronization = syncAll;
            this.dataFileService = new MobeelizerDataFileService(application);
        }

        internal void Sync(MobeelizerSyncCallback callback)
        {
            if (application.CheckSyncStatus() != MobeelizerSyncStatus.STARTED)
            {
                Log.i(TAG, "Send is already running - skipping.");
                callback(MobeelizerSyncStatus.NONE);
            }

            MobeelizerDatabase database = (MobeelizerDatabase)application.GetDatabase();
            IMobeelizerConnectionManager connectionManager = application.GetConnectionManager();

            MobeelizerSyncRequestCallback syncRequestCallback = (ticket) =>
                {
                    try
                    {
                        Log.i(TAG, "Sync request completed: " + ticket + ".");
                        ChangeStatus(MobeelizerSyncStatus.TASK_CREATED);
                        if (!connectionManager.WaitUntilSyncRequestComplete(ticket))
                        {
                            callback(MobeelizerSyncStatus.FINISHED_WITH_FAILURE);
                        }
                        else
                        {
                            Log.i(TAG, "Sync process complete with success.");
                            ChangeStatus(MobeelizerSyncStatus.TASK_PERFORMED);
                            connectionManager.GetSyncData(ticket, (inputFile) =>
                                {
                                    bool success = false;
                                    try
                                    {
                                        ChangeStatus(MobeelizerSyncStatus.FILE_RECEIVED);

                                        success = dataFileService.ProcessInputFile(inputFile, isAllSynchronization);

                                        if (!success)
                                        {
                                            callback(MobeelizerSyncStatus.FINISHED_WITH_FAILURE);
                                        }

                                        connectionManager.ConfirmTask(ticket);
                                        database.ClearModifiedFlag();
                                        application.InternalDatabase.SetInitialSyncAsNotRequired(application.Instance, application.User);
                                    }
                                    catch (IOException e)
                                    {
                                        success = false;
                                        Log.i(TAG, e.Message);
                                    }
                                    catch (InvalidOperationException e)
                                    {
                                        success = false;
                                        Log.i(TAG, e.Message);
                                    }
                                    finally
                                    {
                                        database.UnlockModifiedFlag();
                                        if (success)
                                        {
                                            ChangeStatus(MobeelizerSyncStatus.FINISHED_WITH_SUCCESS);
                                        }
                                        else
                                        {
                                            Log.i(TAG, "Sync process complete with failure.");
                                            ChangeStatus(MobeelizerSyncStatus.FINISHED_WITH_FAILURE);
                                        }
                                    }
                                    callback(success ? MobeelizerSyncStatus.FINISHED_WITH_SUCCESS : MobeelizerSyncStatus.FINISHED_WITH_FAILURE);
                                });
                        }
                    }
                    catch (IOException e)
                    {
                        Log.i(TAG, e.Message);
                        callback(MobeelizerSyncStatus.FINISHED_WITH_FAILURE);
                    }
                    catch (InvalidOperationException e)
                    {
                        Log.i(TAG, e.Message); 
                        callback(MobeelizerSyncStatus.FINISHED_WITH_FAILURE);
                    }
                };

            database.LockModifiedFlag();
            if (isAllSynchronization)
            {
                Log.i(TAG, "Send sync all request.");
                connectionManager.SendSyncAllRequest(syncRequestCallback);
            }
            else
            {
                using (IsolatedStorageFileStream outputFile = GetOuptutFileStream())
                {
                    if (!dataFileService.PrepareOutputFile(outputFile))
                    {
                        Log.i(TAG, "Send file haven't been created.");
                        callback(MobeelizerSyncStatus.FINISHED_WITH_FAILURE);
                    }
                    else
                    {
                        ChangeStatus(MobeelizerSyncStatus.FILE_CREATED);
                        Log.i(TAG, "Send sync request.");
                        connectionManager.SendSyncDiffRequest(outputFile, syncRequestCallback);
                    }
                }
                RemoveOutputFile();
            }
        }

        private void ChangeStatus(MobeelizerSyncStatus mobeelizerSyncStatus)
        {
            // TODO
            this.application.SetSyncStatus(mobeelizerSyncStatus);
        }

        private IsolatedStorageFileStream GetOuptutFileStream()
        {
            IsolatedStorageFileStream outputFile = null;
            using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                String filePath = System.IO.Path.Combine("sync", "syncOutFile.tmp");
                if (!iso.DirectoryExists("sync"))
                {
                    iso.CreateDirectory("sync");
                }
                if (iso.FileExists(filePath))
                {
                    iso.DeleteFile(filePath);
                }

                outputFile = iso.OpenFile(filePath, FileMode.CreateNew, FileAccess.ReadWrite);
            }
            return outputFile;
        }
        private void RemoveOutputFile()
        {
            using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                String filePath = System.IO.Path.Combine("sync", "syncOutFile.tmp");
                if (iso.DirectoryExists("sync"))
                {
                    if (iso.FileExists(filePath))
                    {
                        iso.DeleteFile(filePath);
                    }
                }
            }
        }
    }
}
