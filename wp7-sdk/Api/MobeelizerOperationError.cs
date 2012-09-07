using System;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json.Linq;
using Com.Mobeelizer.Mobile.Wp7.Definition;

namespace Com.Mobeelizer.Mobile.Wp7.Api
{
    /// <summary>
    /// Representation of the operation error.
    /// </summary>
    public class MobeelizerOperationError
    {
        private MobeelizerOperationError() { }

        /// <summary>
        /// Return the code of the error.
        /// </summary>
        /// <value>Code.</value>
        public String Code { get; private set; }

        /// <summary>
        /// Return the readable message for the error.
        /// </summary>
        /// <value>Message.</value>
        public String Message { get; private set; }

        /// <summary>
        /// Return the arguments for message.
        /// </summary>
        /// <value>Arguments.</value>
        public IList<Object> Arguments { get; private set; }

        internal static MobeelizerOperationError Exception(Exception e)
        {
            IList<Object> args = new List<Object>();
            args.Add(e);
            return new MobeelizerOperationError() { Code = "exception", Message = e.Message, Arguments = args };
        }

        internal static MobeelizerOperationError MissingConnectionError()
        {
            return new MobeelizerOperationError() { Code = "missingConnection", Message = "Internet connection required", Arguments = null };
        }

        internal static MobeelizerOperationError ConnectionError()
        {
            return new MobeelizerOperationError() { Code = "connectionFailure", Message = "Connection failure.", Arguments = null };
        }

        internal static MobeelizerOperationError ConnectionError(String message)
        {
            IList<Object> args = new List<Object>();
            args.Add(message);
            return new MobeelizerOperationError() { Code = "connectionFailure", Message = "Connection failure: " + message, Arguments = args };
        }

        internal static MobeelizerOperationError ServerError(JObject jObject)
        {
            String code = (String)jObject["code"];
            String message = (String)jObject["message"];
            JArray array = (JArray)jObject["arguments"];
            IList<Object> args = new List<Object>();
            if (array != null)
            {
                foreach (var item in array)
                {
                    args.Add(item);
                }
            }

            return new MobeelizerOperationError() { Code = code, Message = message, Arguments = args };
        }

        internal static MobeelizerOperationError Other(String message)
        {
            return new MobeelizerOperationError() { Code = "other", Message = message, Arguments = null };
        }

        internal static MobeelizerOperationError UpdateFromSyncError(MobeelizerErrorsHolder errors)
        {
            return new MobeelizerOperationError() { Code = "updateFromSync", Message = "Update entities from sync failiture", Arguments = errors.PrepareErrorArguments() };
        }

        internal static MobeelizerOperationError SyncRejected(String result, String message)
        {
            IList<Object> args = new List<Object>();
            args.Add(result);
            args.Add(message);
            return new MobeelizerOperationError()
            {
                Code = "syncRejected", 
                Message = "Synchronization rejected: result: " + result + ", message: "+ message,
                Arguments = args
            };
        }
    }
}
