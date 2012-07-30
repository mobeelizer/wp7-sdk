
namespace Com.Mobeelizer.Mobile.Wp7.Api
{
    /// <summary>
    /// Modes define how SDK and network connections work.
    /// </summary>
    public enum MobeelizerMode
    {
        /// <summary>
        /// The connections won't be performed. 
        /// </summary>
        DEVELOPMENT,

        /// <summary>
        /// The connections will be established to test instances.
        /// </summary>
        TEST,

        /// <summary>
        /// The connections will be established to production instances.
        /// </summary> 
        PRODUCTION
    }
}
