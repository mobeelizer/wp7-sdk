﻿using System;
using Com.Mobeelizer.Mobile.Wp7.Api;
using System.IO.IsolatedStorage;
using System.IO;
using Com.Mobeelizer.Mobile.Wp7.Database;

namespace Com.Mobeelizer.Mobile.Wp7
{
    internal class MobeelizerSyncServicePerformer
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

        internal MobeelizerOperationError Sync()
        {
            if (application.CheckSyncStatus() != MobeelizerSyncStatus.STARTED)
            {
                Log.i(TAG, "Send is already running - skipping.");
                return  null;
            }

            MobeelizerDatabase database = (MobeelizerDatabase)application.GetDatabase();
            IMobeelizerConnectionManager connectionManager = application.GetConnectionManager();
            String ticket = String.Empty; 
            bool success = false;
            Others.File outputFile= null;
            Others.File inputFile = null;
            try
            {
                database.LockModifiedFlag();
                if (isAllSynchronization)
                {
                    Log.i(TAG, "Send sync all request.");
                    MobeelizerSyncResponse response = connectionManager.SendSyncAllRequest();
                    if (response.Error == null)
                    {
                        ticket = response.Ticket;
                    }
                    else
                    {
                        return response.Error;
                    }
                }
                else
                {
                    outputFile = GetOuptutFile();
                    outputFile.Create();
                    MobeelizerOperationError prepareFileError = dataFileService.PrepareOutputFile(outputFile);
                    if (prepareFileError != null)
                    {
                        Log.i(TAG, "Send file haven't been created.");
                        return prepareFileError;
                    }
                    else
                    {
                        ChangeStatus(MobeelizerSyncStatus.FILE_CREATED, ticket);
                        Log.i(TAG, "Send sync request.");
                        MobeelizerSyncResponse response = connectionManager.SendSyncDiffRequest(outputFile);
                        if(response.Error == null)
                        {
                            ticket = response.Ticket;
                        }
                        else
                        {
                            return response.Error;
                        }
                    }
                }

                Log.i(TAG, "Sync request completed: " + ticket + ".");
                ChangeStatus(MobeelizerSyncStatus.TASK_CREATED, ticket);
                MobeelizerOperationError waitError = connectionManager.WaitUntilSyncRequestComplete(ticket);
                if (waitError != null)
                {
                    return waitError;
                }
                else
                {
                    Log.i(TAG, "Sync process complete with success.");
                    ChangeStatus(MobeelizerSyncStatus.TASK_PERFORMED, ticket);
                    MobeelizerGetSyncDataOperationResult getDataResult = connectionManager.GetSyncData(ticket);
                    if(getDataResult.Error == null)
                    {
                        inputFile = getDataResult.InputFile;
                    }
                    else
                    {
                        return getDataResult.Error;
                    }

                    ChangeStatus(MobeelizerSyncStatus.FILE_RECEIVED, ticket);
                    MobeelizerOperationError processError = dataFileService.ProcessInputFile(inputFile, isAllSynchronization);
                    if (processError != null)
                    {
                        return processError;
                    }
                    else
                    {
                        success = true;
                    }

                    connectionManager.ConfirmTask(ticket);
                    database.ClearModifiedFlag();
                    application.InternalDatabase.SetInitialSyncAsNotRequired(application.Instance, application.User);
                }
            }
            catch (IOException e)
            {
                Log.i(TAG, e.Message);
                return MobeelizerOperationError.Exception(e);
            }
            catch (InvalidOperationException e)
            {
                Log.i(TAG, e.Message);
                return MobeelizerOperationError.Exception(e);
            }
            finally
            {
                if (inputFile != null)
                {
                    inputFile.Delete();
                }

                if (outputFile != null)
                {
                    outputFile.Delete();
                }

                database.UnlockModifiedFlag();
                if (success)
                {
                    ChangeStatus(MobeelizerSyncStatus.FINISHED_WITH_SUCCESS, ticket);
                }
                else
                {
                    Log.i(TAG, "Sync process complete with failure.");
                    ChangeStatus(MobeelizerSyncStatus.FINISHED_WITH_FAILURE, ticket);
                }
            }

            return success ? null : MobeelizerOperationError.Other("Sync process complete with failure.");
        }

        private void ChangeStatus(MobeelizerSyncStatus mobeelizerSyncStatus, String ticket)
        {
            this.application.GetTombstoningManager().SaveSyncTicket(ticket, isAllSynchronization);
            this.application.SetSyncStatus(mobeelizerSyncStatus);
        }

        private Others.File GetOuptutFile()
        {
            String filePath = System.IO.Path.Combine("sync", "syncOutFile.tmp");
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
            return file;
        }

        internal MobeelizerSyncStatus ContinueSync(string ticket)
        {
            throw new NotImplementedException();
            //if (application.CheckSyncStatus() != MobeelizerSyncStatus.STARTED)
            //{
            //    Log.i(TAG, "Send is already running - skipping.");
            //    return MobeelizerSyncStatus.NONE;
            //}

            //MobeelizerDatabase database = (MobeelizerDatabase)application.GetDatabase();
            //IMobeelizerConnectionManager connectionManager = application.GetConnectionManager();
            //bool success = false;
            //Others.File inputFile = null;
            //try
            //{
            //    Log.i(TAG, "Continue sync request: " + ticket + ".");
            //    ChangeStatus(MobeelizerSyncStatus.TASK_CREATED, ticket);
            //    if (!connectionManager.WaitUntilSyncRequestComplete(ticket))
            //    {
            //        return MobeelizerSyncStatus.FINISHED_WITH_FAILURE;
            //    }
            //    else
            //    {
            //        Log.i(TAG, "Sync process complete with success.");
            //        ChangeStatus(MobeelizerSyncStatus.TASK_PERFORMED, ticket);
            //        inputFile = connectionManager.GetSyncData(ticket);
            //        ChangeStatus(MobeelizerSyncStatus.FILE_RECEIVED, ticket);
            //        success = dataFileService.ProcessInputFile(inputFile, isAllSynchronization);
            //        if (!success)
            //        {
            //            return MobeelizerSyncStatus.FINISHED_WITH_FAILURE;
            //        }

            //        connectionManager.ConfirmTask(ticket);
            //        database.ClearModifiedFlag();
            //        application.InternalDatabase.SetInitialSyncAsNotRequired(application.Instance, application.User);
            //    }
            //}
            //catch (IOException e)
            //{
            //    Log.i(TAG, e.Message);
            //    return MobeelizerSyncStatus.FINISHED_WITH_FAILURE;
            //}
            //catch (InvalidOperationException e)
            //{
            //    Log.i(TAG, e.Message);
            //    return MobeelizerSyncStatus.FINISHED_WITH_FAILURE;
            //}
            //finally
            //{
            //    if (inputFile != null)
            //    {
            //        inputFile.Delete();
            //    }

            //    database.UnlockModifiedFlag();
            //    if (success)
            //    {
            //        ChangeStatus(MobeelizerSyncStatus.FINISHED_WITH_SUCCESS, ticket);
            //    }
            //    else
            //    {
            //        Log.i(TAG, "Sync process complete with failure.");
            //        ChangeStatus(MobeelizerSyncStatus.FINISHED_WITH_FAILURE, ticket);
            //    }
            //}
            //return success ? MobeelizerSyncStatus.FINISHED_WITH_SUCCESS : MobeelizerSyncStatus.FINISHED_WITH_FAILURE;
        }
    }
}
