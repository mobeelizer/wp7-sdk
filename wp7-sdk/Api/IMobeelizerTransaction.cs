using System;
using System.Data.Linq;

namespace Com.Mobeelizer.Mobile.Wp7.Api
{
    /// <summary>
    /// Representation of database transaction.
    /// </summary>
    public interface IMobeelizerTransaction : IDisposable
    {
        /// <summary>
        /// Returns a collection of models of praticural type, where type is defined by the T parameter.
        /// </summary>
        /// <typeparam name="T">Model type.</typeparam>
        /// <returns>Collection of models.</returns>
        ITable<T> GetModelSet<T>() where T : MobeelizerWp7Model;

        /// <summary>
        /// Submits all changes created in current transaction to database. 
        /// </summary>
        void SubmitChanges();

        /// <summary>
        /// Closes transaction.
        /// </summary>
        void Close();
    }
}
