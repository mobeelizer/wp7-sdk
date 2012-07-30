
namespace Com.Mobeelizer.Mobile.Wp7.Api
{
    /// <summary>
    /// Representation of the database.
    /// </summary>
    public interface IMobeelizerDatabase
    {
        /// <summary>
        /// Begins new database transaction, remember to close it after all operations. Transaction class 
        /// implements IDisposable interface it is good practise to use using statement. 
        /// <example>
        /// <code>
        /// var database = Mobeelizer.GetDatabase();
        /// using(var transaction = database.BeginTransaction())
        /// {
        ///     // Operations on database.
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <returns>Opened transaction.</returns>
        IMobeelizerTransaction BeginTransaction();
    }
}
