using System;

namespace Com.Mobeelizer.Mobile.Wp7.Api
{
    /// <summary>
    /// Login result.
    /// </summary>
    public class MobeelizerLoginResult
    {
        private MobeelizerLoginStatus status;

        private Exception exception;

        internal MobeelizerLoginResult(MobeelizerLoginStatus status)
        {
            this.status = status;
        }

        internal MobeelizerLoginResult(Exception e)
        {
            this.status = MobeelizerLoginStatus.OTHER_FAILURE;
            this.exception = e;
        }

        /// <summary>
        /// Returns login status. If login process generates an unexpected exception, execution of  this method throws it.
        /// </summary>
        /// <returns>Login status.</returns>
        /// <exception cref="Microsoft.Practices.Mobile.Configuration.ConfigurationException">
        ///  Configuration exception can be thrown when models class are different than 'definition xml' defines.
        /// </exception>
        public MobeelizerLoginStatus GetLoginStatus()
        {
            if (this.exception != null)
            {
                throw exception;
            }

            return this.status;
        }
    }
}
