using System;
using Com.Mobeelizer.Mobile.Wp7.Api;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System.Threading;

namespace Com.Mobeelizer.Mobile.Wp7
{
    internal class MobeelizerTombstoningManager
    {
        private const String TAG = "mobeelizer:tomstoningmanager";

        private const String DATA_DIRECTORY = "mobeelzer";

        private const String DATA_FILE = "MobeelizerTomstoningState.xml";

        private MobeelizerApplication application;

        private String syncTicket;

        private bool isAllSynchronization;

        internal MobeelizerTombstoningManager(MobeelizerApplication application)
        {
            this.application = application;
        }

        internal void SaveSyncTicket(String ticket, bool isAllSynchronization)
        {
            this.syncTicket = ticket;
            this.isAllSynchronization = isAllSynchronization;
        }

        internal void SaveApplicationState()
        {
            MobeelizerTombstoningState state = new MobeelizerTombstoningState();
            if (application.IsLoggedIn)
            {
                state.LoggedIn = true;
                state.User = application.User;
                state.Password = application.Password; // TODO: encrypt password
                state.Instance = application.Instance;
                state.SyncStatus = application.CheckSyncStatus();
                state.SyncTicket = this.syncTicket;
            }
            else
            {
                state.LoggedIn = false;
            }

            using (var iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!iso.DirectoryExists(DATA_DIRECTORY))
                {
                    iso.CreateDirectory(DATA_DIRECTORY);
                }

                using (IsolatedStorageFileStream stream = iso.OpenFile(System.IO.Path.Combine(DATA_DIRECTORY, DATA_FILE), System.IO.FileMode.CreateNew))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(MobeelizerTombstoningState));
                    serializer.Serialize(stream, state);
                }
            }

            Log.i(TAG, "Mobeelizer application state saved");
        }

        internal void RestoreApplicationState()
        {
            MobeelizerTombstoningState state;
            using (var iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!iso.DirectoryExists(DATA_DIRECTORY))
                {
                    return;
                }

                if (!iso.FileExists(System.IO.Path.Combine(DATA_DIRECTORY, DATA_FILE)))
                {
                    return;
                }

                using (IsolatedStorageFileStream stream = iso.OpenFile(System.IO.Path.Combine(DATA_DIRECTORY, DATA_FILE), System.IO.FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(MobeelizerTombstoningState));
                    state = serializer.Deserialize(stream) as MobeelizerTombstoningState;
                }
            }

            if(state != null)
            {
                Log.i(TAG, "Restoring saved application state.");
                if (state.LoggedIn)
                {
                    if (application.OfflineLogin(state.Instance, state.User, state.Password) == null)
                    {
                        switch(state.SyncStatus)
                        {
                            case MobeelizerSyncStatus.TASK_CREATED:
                            case MobeelizerSyncStatus.TASK_PERFORMED:
                            case MobeelizerSyncStatus.FILE_RECEIVED:
                                Thread continueSyncThread = new Thread(new ThreadStart(() =>
                                {
                                    application.SetSyncStatus(MobeelizerSyncStatus.STARTED);
                                    new MobeelizerSyncServicePerformer(Mobeelizer.Instance, state.IsAllSynchronization).ContinueSync(state.SyncTicket);
                                }));
                                continueSyncThread.Name = "Resume syn thread";
                                continueSyncThread.Start();
                                break;
                        }
                    }
                }
            }
        }

        internal void ClearSavedState()
        {
            using (var iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!iso.DirectoryExists(DATA_DIRECTORY))
                {
                    return;
                }

                if (iso.FileExists(System.IO.Path.Combine(DATA_DIRECTORY, DATA_FILE)))
                {
                    iso.DeleteFile(System.IO.Path.Combine(DATA_DIRECTORY, DATA_FILE));
                }
            }

            Log.i(TAG, "Saved application state removed.");
        }

        internal void ClearAndUnlockSavedState()
        {
            MobeelizerTombstoningState state;
            using (var iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!iso.DirectoryExists(DATA_DIRECTORY))
                {
                    return;
                }

                if (!iso.FileExists(System.IO.Path.Combine(DATA_DIRECTORY, DATA_FILE)))
                {
                    return;
                }

                using (IsolatedStorageFileStream stream = iso.OpenFile(System.IO.Path.Combine(DATA_DIRECTORY, DATA_FILE), System.IO.FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(MobeelizerTombstoningState));
                    state = serializer.Deserialize(stream) as MobeelizerTombstoningState;
                }
            }

            if (state != null)
            {
                Log.i(TAG, "Clearing modyfication flag.");
                if (state.LoggedIn)
                {
                    if (application.OfflineLogin(state.Instance, state.User, state.Password) == null)
                    {
                        switch (state.SyncStatus)
                        {
                            case MobeelizerSyncStatus.TASK_CREATED:
                            case MobeelizerSyncStatus.TASK_PERFORMED:
                            case MobeelizerSyncStatus.FILE_RECEIVED:
                                application.GetDatabase().UnlockModifiedFlag();
                                break;
                        }

                        application.Logout();
                    }
                }
            }

            this.ClearSavedState();
        }
    }
}
