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

        public IMobeelizerAuthenticateResponse Authenticate(string user, string password, object remoteNotifycationToken)
        {
            throw new NotImplementedException();
        }

        public IMobeelizerAuthenticateResponse Authenticate(string user, string password)
        {
            WebRequest request = WebRequest.Create(GetUrl("/authenticate"));
            request.Method = "GET";
            SetHeaders(request, false, false);
            request.Headers["mas-user-name"] = user;
            request.Headers["mas-user-password"] = password;
            try
            {

                JObject result = new Synchronizer().GetJsonResponse(request);
                String status = (String)result["status"];
                if (status == "OK")
                {
                    JObject content = (JObject)result["content"];
                    return new MobeelizerAuthenticateResponse((String)content["instanceGuid"], (String)content["role"]);
                }
                else
                {
                    return null;
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
                            return null;
                        }
                        else
                        {
                            throw new InvalidOperationException(str, e);
                        }
                    }
                }
            }
                 
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

        public String SendSyncAllRequest()
        {
            WebRequest request = WebRequest.Create(GetUrl("/synchronizeAll"));
            request.Method = "POST";
            SetHeaders(request, true, true);
            try
            {
                JObject response = new Synchronizer().GetJsonResponse(request);
                return (String)response["content"];
            }
            catch (WebException e)
            {
                String message;
                using (Stream stream = e.Response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        message = reader.ReadToEnd();
                    }

                }
                throw new IOException(message, e);
            }
            catch (NullReferenceException)
            {
                throw new IOException("Server not respond.");
            }
        }


        public String SendSyncDiffRequest(Others.File outputFile)
        { 
            String boundary = DateTime.Now.Ticks.ToString();

            WebRequest request = WebRequest.Create(GetUrl("/synchronize"));
            request.Method = "POST";
            request.ContentType = String.Format("multipart/form-data; boundary={0}",boundary);

            using (Stream requestStream = new Synchronizer().GetRequestStream(request))
            {
                String header = String.Format("--{0}\r\nContent-Disposition: form-data; name=\"file\"; filename=\"file\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                requestStream.Write(Encoding.UTF8.GetBytes(header), 0, header.Length);
                using (IsolatedStorageFileStream stream = outputFile.OpenToRead())
                {
                    int lenght = (int)stream.Length;
                    byte[] bytes = new byte[lenght];
                    stream.Read(bytes, 0, lenght);
                    requestStream.Write(bytes, 0, lenght);    
                }
                string footer = "\r\n--" + boundary + "--\r\n";
                byte[] footerbytes = Encoding.UTF8.GetBytes(footer);
                requestStream.Write(footerbytes, 0, footerbytes.Length);
            }
            SetHeaders(request, false, true);
            try
            {
                JObject response = new Synchronizer().GetJsonResponse(request);
                return (String)response["content"];
            }
            catch (WebException e)
            {
                String message;
                using (Stream stream = e.Response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        message = reader.ReadToEnd();
                    }

                }
                throw new IOException(message, e);
            }
            catch (NullReferenceException)
            {
                throw new IOException("Server not respond.");
            }
        }

        public Others.File GetSyncData(string ticket)
        {
            WebRequest request = WebRequest.Create(GetUrl(String.Format("/data?ticket={0}", ticket)));
            request.Method = "GET";
            SetHeaders(request, false, true);
            try
            {
                Others.File inputFile = GetInputFileStream();
                inputFile.Create();
                using (Stream stream = new Synchronizer().GetResponseData(request))
                {
                    using (IsolatedStorageFileStream inputStream = inputFile.OpenToWrite())
                    {
                        stream.CopyTo(inputStream);
                    }
                }
                return inputFile;
            }
            catch (WebException e)
            {
                String message;
                using (Stream stream = e.Response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        message = reader.ReadToEnd();
                    }

                }
                throw new IOException(message, e);
            }
            catch (NullReferenceException)
            {
                throw new IOException("Server not respond.");
            }
        }

        public void ConfirmTask(string ticket)
        {
            WebRequest request = WebRequest.Create(GetUrl(String.Format("/confirm?ticket={0}", ticket)));
            request.Method = "POST";
            SetHeaders(request, false, true);
            JObject result = new Synchronizer().GetJsonResponse(request);
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
                WebRequest request = WebRequest.Create(GetUrl(String.Format("/checkStatus?ticket={0}&aaa={1}", ticket, DateTime.Now.Ticks)));

                (request as HttpWebRequest).AllowReadStreamBuffering = false;
                request.Method = "GET";
                SetHeaders(request, false, true);

                JObject jResult = new Synchronizer().GetJsonResponse(request);
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

        private Others.File GetInputFileStream()
        {
            String filePath = System.IO.Path.Combine("sync", "input.tmp");
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

            return new Others.File(filePath);
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
            request.Headers["Cache-Control"] = "no-cache";

            if (setUserPassword)
            {
                request.Headers["mas-user-name"] = application.User;
                request.Headers["mas-user-password"] = application.Password;
            }

            request.Headers["mas-sdk-version"] = Mobeelizer.VERSION;
        }
    }

    internal class Synchronizer
    {
        private ManualResetEvent allDone = new ManualResetEvent(false);

        internal JObject GetJsonResponse(WebRequest request)
        {
            JObject result = null;
            WebException exception = null;
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
                        exception = e;
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
            allDone.WaitOne(TimeSpan.FromSeconds(30));
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

        internal Stream GetResponseData(WebRequest request)
        {
            Stream stream = null;
            WebException exception = null;
            try
            {
                request.BeginGetResponse(a =>
                {
                    try
                    {
                        using (WebResponse response = request.EndGetResponse(a))
                        {
                            stream = response.GetResponseStream();
                        }
                    }
                    catch (WebException e)
                    {
                        exception = e;
                    }
                    allDone.Set();
                }, null);
            }
            catch (WebException e)
            {
                throw new IOException(e.Message, e);

            }

            allDone.WaitOne(TimeSpan.FromSeconds(30));
            if (exception != null)
            {
                throw exception;
            }
            return stream;
        }
    }
}
