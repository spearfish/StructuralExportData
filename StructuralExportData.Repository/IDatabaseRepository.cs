using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructuralExportData.Repository
{
    public interface IDatabaseRepository
    {
        /// <summary>
        /// Create Database 
        /// </summary>
        /// <remarks>This will create a database</remarks>
        void Create(IReadOnlyList<DataElement> dataElements, string type);

        /// <summary>
        /// Read Database 
        /// </summary>
        /// <remarks>Read the database</remarks>
        void Read();

        /// <summary>
        /// Update the database
        /// </summary>
        /// <remarks>Update the database</remarks>
        void Write(IReadOnlyList<DataElement> dataElements);

        /// <summary>
        /// Delete the database
        /// </summary>
        /// <remarks>Delete the database</remarks>
        void Delete();
    }
}
