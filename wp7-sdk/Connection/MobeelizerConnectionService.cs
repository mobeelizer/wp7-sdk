using System;
using System.IO;
using System.Net;
using Com.Mobeelizer.Mobile.Wp7.Api;
using Newtonsoft.Json.Linq;
using System.IO.IsolatedStorage;
using System.Threading;
using System.Text;
using Newtonsoft.Json;

namespace Com.Mobeelizer.Mobile.Wp7.Connection
{
    internal class MobeelizerConnectionService : IMobeelizerConnectionService
    {
        private const String TAG = "mobeelizer:connectionService";

        private const String DEFAULT_PRODUCTION_URL = "https://cloud.mobeelizer.com/sync/v2";

        private const String DEFAULT_TEST_URL = "https://cloud.mobeelizer.com/sync/v2";

        private MobeelizerApplication application;

        private IMobeelizerNotificationTokenConverter tokenConverter;

        internal MobeelizerConnectionService(MobeelizerApplication application)
        {
            this.application = application;
            this.tokenConverter = new MobeelizerNotificationTokenConverter();
        }

        public IMobeelizerAuthenticateResponse Authenticate(string user, string password, String notificationChanelUri)
        {
            WebRequest request = WebRequest.Create(GetUrl("/authenticate?cache="+Guid.NewGuid().ToString()));
            request.Method = "GET";
            SetHeaders(request, false, false);
            request.Headers["mas-user-name"] = user;
            request.Headers["mas-user-password"] = password;
            try
            {

                MobeelizerResponse result = new Synchronizer().GetResponse(request);
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    JObject jObject = (result as MobeelizerJsonResponse).Json;
                    return new MobeelizerAuthenticateResponse((String)jObject["instanceGuid"], (String)jObject["role"]);
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
            catch (JsonException e)
            {
                throw new InvalidOperationException(e.Message, e);
            }
        }

        public IMobeelizerAuthenticateResponse Authenticate(string user, string password)
        {
            return this.Authenticate(user, password, null);
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
                MobeelizerResponse response = new Synchronizer().GetResponse(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return (response as MobeelizerTicketResponse).Ticket;
                }
                else
                {
                    throw new IOException(response.StatusCode.ToString() + ": " + (response as MobeelizerJsonResponse).Json.ToString());
                }
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
                MobeelizerResponse response = new Synchronizer().GetResponse(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return (response as MobeelizerTicketResponse).Ticket;
                }
                else
                {
                    throw new IOException(response.StatusCode.ToString() + ": " + (response as MobeelizerJsonResponse).Json.ToString());
                }
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
            MobeelizerResponse result = new Synchronizer().GetResponse(request);
            if (result == null || result.StatusCode != HttpStatusCode.OK)
            {
                throw new IOException("Unable to confirm synchronization. "+ result.StatusCode.ToString() + ": " + (result as MobeelizerJsonResponse).Json.ToString());
            }
        }

        public bool WaitUntilSyncRequestComplete(string ticket)
        {
            MobeelizerSynchronizationStatus sycStatus = MobeelizerSynchronizationStatus.REJECTED;
            for (int i = 0; i < 100; i++)
            {
                WebRequest request = WebRequest.Create(GetUrl(String.Format("/checkStatus?ticket={0}&aaa={1}", ticket, DateTime.Now.Ticks)));
                request.Method = "GET";
                SetHeaders(request, false, true);

                MobeelizerResponse result = new Synchronizer().GetResponse(request);
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    String strSyncStatus = (String)(result as MobeelizerJsonResponse).Json["status"];
                    sycStatus = (MobeelizerSynchronizationStatus)Enum.Parse(typeof(MobeelizerSynchronizationStatus), strSyncStatus, true);
                }
                else
                {
                    throw new IOException(result.StatusCode.ToString() + ": " + (result as MobeelizerJsonResponse).Json.ToString());
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

        public void RegisterForRemoteNotifications(string channelUri)
        {
            String token = this.tokenConverter.Convert(channelUri);
            WebRequest request = WebRequest.Create(GetUrl(String.Format("/registerPushToken?deviceToken={0}&deviceType=wp7", token)));
            request.Method = "POST";
            SetHeaders(request, true, true);
            try
            {
                MobeelizerResponse result = new Synchronizer().GetResponse(request);
                if (result == null || result.StatusCode != HttpStatusCode.OK)
                {
                    throw new IOException("Unable to register for remote notification.");
                }
            }
            catch (WebException e)
            {
                throw new IOException(e.Message, e);
            }

            Log.i(TAG, "Registered for remote notifications with chanel: " + channelUri);
        }

        public void UnregisterForRemoteNotifications(string channelUri)
        {
            String token = String.Format("{0:X}", channelUri);
            WebRequest request = WebRequest.Create(GetUrl(String.Format("/unregisterPushToken?deviceToken={0}&deviceType=wp7", channelUri)));
            request.Method = "POST";
            SetHeaders(request, true, true);
            try
            {
                MobeelizerResponse result = new Synchronizer().GetResponse(request);
                if (result == null || result.StatusCode != HttpStatusCode.OK)
                {
                    throw new IOException("Unable to unregister for remote notification.");
                }
            }
            catch (WebException e)
            {
                throw new IOException(e.Message, e);
            }

            Log.i(TAG, "Unregistered for remote notifications with chanel: " + channelUri);
        }

        public void SendRemoteNotification(string device, string group, System.Collections.Generic.IList<string> users, System.Collections.Generic.IDictionary<string, string> notification)
        {
            try
            {
                JObject jobject = new JObject();
                StringBuilder logBuilder = new StringBuilder();
                logBuilder.Append("Sent remote notification ").Append(notification).Append(" to");
                if (device != null)
                {
                    jobject.Add("device", device);
                    logBuilder.Append(" device: ").Append(device);
                }
                if (group != null)
                {
                    jobject.Add("group", group);
                    logBuilder.Append(" group: ").Append(group);
                }
                if (users != null)
                {
                    jobject.Add("users", new JArray(users));
                    logBuilder.Append(" users: ").Append(users);
                }
                if (device == null && group == null && users == null)
                {
                    logBuilder.Append(" everyone");
                }
                JObject jnotification = new JObject();
                foreach (var notify in notification)
                {
                    jnotification.Add(notify.Key, notify.Value);
                }

                jobject.Add("notification", jnotification);

                WebRequest request = WebRequest.Create(GetUrl("/push"));
                request.Method = "POST";
                using (Stream stream = new Synchronizer().GetRequestStream(request))
                {
                    byte[] notificationMessage = Encoding.UTF8.GetBytes(jobject.ToString());
                    stream.Write(notificationMessage, 0, notificationMessage.Length);
                }

                SetHeaders(request, true, true);
                MobeelizerResponse result = new Synchronizer().GetResponse(request);
                if (result.StatusCode != HttpStatusCode.OK)
                {
                    throw new IOException((result as MobeelizerJsonResponse).Json.ToString());
                }

                Log.i(TAG, logBuilder.ToString());
            }
            catch (JsonException)
            {
                //  throw new IOException(e.getMessage(), e);
            }
        }

        private abstract class MobeelizerResponse
        {
            public HttpStatusCode StatusCode { get; set; }
        }

        private class MobeelizerJsonResponse : MobeelizerResponse
        {
            public JObject Json { get; set; }
        }

        private class MobeelizerTicketResponse : MobeelizerResponse
        {
            public String Ticket { get; set; }
        }

        private class Synchronizer
        {
            private ManualResetEvent allDone = new ManualResetEvent(false);

            internal MobeelizerResponse GetResponse(WebRequest request)
            {
                MobeelizerResponse result = null;
                Exception exception = null;
                try
                {
                    request.BeginGetResponse(a =>
                    {
                        try
                        {
                            using (HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(a))
                            {
                                using (Stream str = response.GetResponseStream())
                                {
                                    try
                                    {
                                        JObject json = GetJsonObject(str);
                                        result = new MobeelizerJsonResponse() { Json = json };
                                    }
                                    catch (JsonReaderException)
                                    {
                                            str.Seek(0, SeekOrigin.Begin);
                                            result = new MobeelizerTicketResponse() { Ticket = new StreamReader(str).ReadToEnd() }; 
                                    }
                                }

                                result.StatusCode = (response).StatusCode;
                            }
                        }
                        catch (WebException e)
                        {
                            using (WebResponse response = e.Response)
                            {
                                using (Stream str = response.GetResponseStream())
                                {
                                    result = new MobeelizerJsonResponse() { Json = GetJsonObject(str) };
                                }
                                result.StatusCode = (response as HttpWebResponse).StatusCode;
                            }
                        }
                        catch (JsonReaderException e)
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

            private JObject GetJsonObject(Stream stream)
            {
                JObject obj = new JObject();
                StreamReader reader = new StreamReader(stream);
                String str = reader.ReadToEnd();
                obj = JObject.Parse(str);
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
}
