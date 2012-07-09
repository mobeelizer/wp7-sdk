using System;
using System.IO;
using System.Net;
using Com.Mobeelizer.Mobile.Wp7.Api;
using Newtonsoft.Json.Linq;
using System.IO.IsolatedStorage;
using System.Threading;
using System.Text;

namespace Com.Mobeelizer.Mobile.Wp7.Connection
{
    public class MobeelizerConnectionService : IMobeelizerConnectionService
    {
        private const String DEFAULT_PRODUCTION_URL = "http://cloud.mobeelizer.com/sync";

        private const String DEFAULT_TEST_URL = "http://cloud.mobeelizer.com/sync";

        private MobeelizerApplication application;

        public MobeelizerConnectionService(MobeelizerApplication application)
        {
            this.application = application;
        }

        public void Authenticate(string user, string password, object remoteNotifycationToken, MobeelizerAuthenticateResponseCallback callback)
        {
            throw new NotImplementedException();
        }

        public void Authenticate(string user, string password, MobeelizerAuthenticateResponseCallback callback)
        {
            WebRequest request = WebRequest.Create(GetUrl("/authenticate"));
            request.Method = "GET";
            SetHeaders(request, false, false);
            request.Headers["mas-user-name"] = user;
            request.Headers["mas-user-password"] = password;
            request.BeginGetResponse(
                 a =>
                 {
                     try
                     {
                         using (WebResponse response = request.EndGetResponse(a))
                         {
                             JObject jResult = (JObject)GetJsonObject(response);
                             String status = (String)jResult["status"];
                             if (status == "OK")
                             {
                                 JObject content = (JObject)jResult["content"];
                                 callback(new MobeelizerAuthenticateResponse((String)content["instanceGuid"], (String)content["role"]));
                             }
                             else
                             {
                                 callback(null);
                             }
                         }
                     }
                     catch (WebException e)
                     {
                         using (WebResponse response = e.Response)
                         {
                             using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                             {
                                 String str = reader.ReadToEnd();
                                 if (str.Contains("Authentication failure"))
                                 {
                                     callback(null);
                                 }
                                 else
                                 {
                                     throw e;
                                 }
                             }
                         }
                     }
                 }
                 , null);
        }

        private JObject GetJsonObject(WebResponse response)
        {
            JObject obj = new JObject();
            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                String str = reader.ReadToEnd();
                obj = JObject.Parse(str);
            }
            return obj;
        }

        private String GetUrl(String url)
        {
            return Url + url;
        }

        private String Url
        {
            get
            {
                if (application.Url != null)
                {
                    return application.Url;
                }
                else if (application.Mode == MobeelizerMode.PRODUCTION)
                {
                    return DEFAULT_PRODUCTION_URL;
                }
                else
                {
                    return DEFAULT_TEST_URL;
                }
            }
        }

        private void SetHeaders(WebRequest request, bool setJsonContentType, bool setUserPassword)
        {
            if (setJsonContentType)
            {
                request.ContentType = "application/json";
            }

            request.Headers["mas-vendor-name"] = application.Vendor;
            request.Headers["mas-application-name"] = application.Application;
            request.Headers["mas-application-instance-name"] = application.Instance;
            request.Headers["mas-definition-digest"] = application.Digest;
            request.Headers["mas-device-name"] = application.Device;
            request.Headers["mas-device-identifier"] = application.DeviceIdentifier;
            if (setUserPassword)
            {
                request.Headers["mas-user-name"] = application.User;
                request.Headers["mas-user-password"] = application.Password;
            }

            request.Headers["mas-sdk-version"] = Mobeelizer.VERSION;
        }

        public void SendSyncAllRequest(MobeelizerSyncRequestCallback callback)
        {
            WebRequest request = WebRequest.Create(GetUrl("/synchronizeAll"));
            request.Method = "POST";
            SetHeaders(request, true, true);
            request.BeginGetResponse(
                a =>
                {
                    try
                    {
                        using (WebResponse response = request.EndGetResponse(a))
                        {
                            String content = (String)GetJsonObject(response)["content"];
                            callback("41"/*content) TODO */);
                        }
                    }
                    catch (WebException e)
                    {
                        // TODO
                        throw new IOException(e.Message);
                    }
                }
                , null);
        }


        public void SendSyncDiffRequest(IsolatedStorageFileStream outputFile, MobeelizerSyncRequestCallback callback)
        { //TODO test it later
            WebRequest request = WebRequest.Create(GetUrl("/synchronize"));
            request.Method = "POST";
            request.ContentType = "multipart/form-data";
            using (Stream requestStream = new Synchronizer().GetRequestStream(request))
            {
                outputFile.CopyTo(requestStream);
            }
            SetHeaders(request, false, true);
            request.BeginGetResponse(
                a =>
                {
                    try
                    {
                        using (WebResponse response = request.EndGetResponse(a))
                        {
                            String content = (String)GetJsonObject(response)["content"];
                            callback(content);
                        }
                    }
                    catch (WebException e)
                    {
                        throw new IOException(e.Message, e);
                    }
                }
                , null);
        }

        private IsolatedStorageFileStream GetInputFileStream()
        {
            IsolatedStorageFileStream stream = null;
            using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                String filePath = System.IO.Path.Combine("sync", "input.tmp");
                if(!iso.DirectoryExists("sync"))
                {
                    iso.CreateDirectory("sync");
                }
                if(iso.FileExists(filePath))
                {
                    iso.DeleteFile(filePath);
                }

                stream = iso.OpenFile(filePath, FileMode.CreateNew, FileAccess.ReadWrite);
            }

            return stream; 
        }

        private void RemoveInputFile()
        {
            using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                String filePath = System.IO.Path.Combine("sync", "input.tmp");
                if (iso.DirectoryExists("sync"))
                {
                    if (iso.FileExists(filePath))
                    {
                        iso.DeleteFile(filePath);
                    }
                }
            }
        }

        public void GetSyncData(string ticket, MobeelizerGetSyncDataCallback callback)
        {
            WebRequest request = WebRequest.Create(GetUrl(String.Format("/data?ticket={0}", ticket)));
            request.Method = "GET";
            SetHeaders(request, false, true);
            request.BeginGetResponse(
                a =>
                {
                    try
                    {
                        using (WebResponse response = request.EndGetResponse(a))
                        {
                            using (IsolatedStorageFileStream inputFile = GetInputFileStream())
                            {
                                using (Stream stream = response.GetResponseStream())
                                {
                                    stream.CopyTo(inputFile);
                                }
                                callback(inputFile);
                            }
                            RemoveInputFile();
                        }
                    }
                    catch (WebException e)
                    {
                        // TODO analize it 
                        throw new IOException(e.Message);
                    }
                }
                , null);
        }

        public void ConfirmTask(string ticket)
        {
            WebRequest request = WebRequest.Create(GetUrl(String.Format("/confirm?ticket={0}", ticket)));
            request.Method = "POST";
            SetHeaders(request, false, true);
            JObject result = new Synchronizer().GetResponse(request);
            if (result == null || (String)result["status"] != "OK")
            {
                throw new IOException("Unable to confirm synchronization.");
            }
        }

        public bool WaitUntilSyncRequestComplete(string ticket)
        {
            MobeelizerSynchronizationStatus sycStatus = MobeelizerSynchronizationStatus.REJECTED;
            for (int i = 0; i < 100; i++)
            {
                WebRequest request = WebRequest.Create(GetUrl(String.Format("/checkStatus?ticket={0}", ticket)));
                request.Method = "GET";
                SetHeaders(request, false, true);

                JObject jResult = new Synchronizer().GetResponse(request);
                String status = (String)jResult["status"];
                if (status == "OK")
                {
                    JObject content = (JObject)jResult["content"];
                    String strSyncStatus = (String)content["status"];
                    sycStatus = (MobeelizerSynchronizationStatus)Enum.Parse(typeof(MobeelizerSynchronizationStatus), strSyncStatus, true);
                }
                else
                {
                    throw new IOException(jResult.ToString());
                }

                if (sycStatus == MobeelizerSynchronizationStatus.REJECTED || sycStatus == MobeelizerSynchronizationStatus.CONFIRMED)
                {
                    return false;
                }
                else if (sycStatus == MobeelizerSynchronizationStatus.FINISHED)
                {
                    return true;
                }

                try
                {
                    Thread.Sleep(5000);
                }
                catch (Exception e)
                {
                    throw new IOException(e.Message, e);
                }
            }

            return false;
        }
    }

    internal class Synchronizer
    {
        private ManualResetEvent allDone = new ManualResetEvent(false);

        internal JObject GetResponse(WebRequest request)
        {
            JObject result = null;
            IOException exception = null;
            try
            {
                request.BeginGetResponse(a =>
                {
                    try
                    {
                        using (WebResponse response = request.EndGetResponse(a))
                        {
                            result = (JObject)GetJsonObject(response);
                        }
                    }
                    catch (WebException e)
                    {
                        exception = new IOException(e.Message, e);
                    }
                    allDone.Set();
                }, null);
            }
            catch (WebException e)
            {
                throw new IOException(e.Message, e);
             
            }

            allDone.WaitOne();
            if (exception != null)
            {
                throw exception;
            }
            return result;
        }

        internal Stream GetRequestStream(WebRequest request)
        {
            Stream stream = null;
            request.BeginGetRequestStream(a =>
            {
                stream = request.EndGetRequestStream(a);
                allDone.Set();
            }, null);
            allDone.WaitOne();
            return stream;
        }

        private JObject GetJsonObject(WebResponse response)
        {
            JObject obj = new JObject();
            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                String str = reader.ReadToEnd();
                obj = JObject.Parse(str);
            }

            return obj;
        }
    }
}
