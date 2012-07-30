
namespace Com.Mobeelizer.Mobile.Wp7.Api
{
    /// <summary>
    /// The result of creation a user session.
    /// </summary>
    public enum MobeelizerLoginStatus
    {
        /// <summary>
        /// The user session has been successfully created. 
        /// </summary>
        OK,
        
        /// <summary>
        /// Login, password and instance do not match to any existing users.
        /// </summary>
        AUTHENTICATION_FAILURE,

        /// <summary>
        /// Unknown error. Look for the explanation in the instance logs and the application logs.
        /// </summary>
        OTHER_FAILURE,
    
        /// <summary>
        /// Connection error. Look for the explanation in the application logs.
        /// </summary>
        CONNECTION_FAILURE,
    
        /// <summary>
        /// Missing connection. First login requires active Internet connection.
        /// </summary>
        MISSING_CONNECTION_FAILURE
    }
}