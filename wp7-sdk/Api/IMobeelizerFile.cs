using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.IsolatedStorage;

namespace Com.Mobeelizer.Mobile.Wp7.Api
{
    /// <summary>
    /// Mobeelizer file definition.
    /// </summary>
    public interface IMobeelizerFile
    {
        /// <summary>
        /// Unique file identificator.
        /// </summary>
        String Guid { get; }

        /// <summary>
        /// File name.
        /// </summary>
        String Name { get; }

        /// <summary>
        /// File stream.
        /// </summary>
        /// <returns>Content of the file.</returns>
        IsolatedStorageFileStream GetStream();
    }
}
