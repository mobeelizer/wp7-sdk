using System;
using System.Net;
using System.Threading;
using System.IO;
using System.Windows;

namespace wp7_sdk_unitTests.Helpers.Mock
{
    public class UTWebRequest : WebRequest
    {
        private static int ticket = 0;

        public static String SyncData { get; set; }

        public override string ContentType { get; set; }

        public override string Method { get; set; }

        public override Uri RequestUri
        {
            get
            {
                return uri;
            }
        }
        WebHeaderCollection headerCollection = new WebHeaderCollection();

        public override WebHeaderCollection Headers
        {
            get
            {
                return this.headerCollection;
            }
            set
            {
                this.headerCollection = value;
            }
        }

        private Uri uri;

        public UTWebRequest(Uri uri)
        {
            this.uri = uri;
        }

        public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
        {
            UTAsyncResult result = new UTAsyncResult();
            Thread thread = new Thread(new ThreadStart(() =>
            {
                callback(result);
            }));
            thread.Start();
            return result;
        }

        public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
        {
            UTAsyncResult result = new UTAsyncResult();
            Thread thread = new Thread(new ThreadStart(() =>
            {
                callback(result);
            }));
            thread.Start();
            return result;
        }

        public override Stream EndGetRequestStream(IAsyncResult asyncResult)
        {
            MemoryStream stream = new MemoryStream();
            return stream;
        }

        public override WebResponse EndGetResponse(IAsyncResult asyncResult)
        {
            Stream stream = null;
            if (Method == "GET")
            {
                switch (this.RequestUri.AbsolutePath)
                {
                    case "/sync/authenticate":
                        stream = this.ValidateUserAndPassword();
                        break;
                    case "/sync/data":
                        stream = GetSyncData();
                        break;
                    case "/sync/checkStatus":
                        stream = new MemoryStream();
                        StreamWriter writer = new StreamWriter(stream);
                        writer.Write("{\r\n    \"status\": \"FINISHED\",\r\n    \"result\": null,\r\n    \"message\": null\r\n  }");
                        writer.Flush();
                        stream.Seek(0, SeekOrigin.Begin);
                        break;
                    default:
                        stream = new MemoryStream();
                        break;
                }
            }
            else if (Method == "POST")
            {
                switch (this.RequestUri.AbsolutePath)
                {
                    case "/sync/synchronizeAll":
                        ++ticket;
                        stream = new MemoryStream();
                        StreamWriter writer = new StreamWriter(stream);
                        writer.Write(ticket);
                        writer.Flush();
                        stream.Seek(0, SeekOrigin.Begin);
                        break;
                    case "/sync/confirm":
                        stream = new MemoryStream();
                        StreamWriter writer1 = new StreamWriter(stream);
                        writer1.Write("{\r\n  \"status\": \"OK\",\r\n  \"content\": null\r\n}");
                        writer1.Flush();
                        stream.Seek(0, SeekOrigin.Begin);
                        break;
                    default:
                        stream = new MemoryStream();
                        break;
                }
            }

            return new UTWebResponse(stream);
        }

        private Stream GetSyncData()
        {
            String filePath = System.IO.Path.Combine("Resources//SyncData", SyncData);
            MemoryStream stream = new MemoryStream();
            try
            {
                var resource = Application.GetResourceStream(new Uri(filePath, UriKind.Relative));
                resource.Stream.CopyTo(stream);
                stream.Seek(0, SeekOrigin.Begin);
            }
            catch
            { }

            return stream;
        }

        private Stream ValidateUserAndPassword()
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            if (this.Headers["mas-user-name"] == "user" && this.Headers["mas-user-password"] == "password")
            {
                writer.Write("{\r\n    \"role\": \"users-mobile\",\r\n    \"instanceGuid\": \"e22e4ce6-1583-49b1-bb46-8dd097589c09\"\r\n  }");
            }
            else
            {
                writer.Write("{\r\n    \"messageCode\": \"authenticationFailure\",\r\n    \"message\": \"Authentication failure for user 'usefr', vendor 'mobeelizer' and application 'test1'#'test'\",\r\n    \"arguments\": null\r\n  }");
            }
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }
    }
}
