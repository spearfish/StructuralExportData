using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructuralExportData.Repository
{
    public interface IElementRepository
    {
        /// <summary>
        /// 
        /// </summary>
        void CreateElement(Dictionary<string, List<Tuple<string, string>>> dictionary, List<string> CompleteParameters);

        /// <summary>
        /// 
        /// </summary>
        IReadOnlyList<DataElement> ReadElement();

        /// <summary>
        /// 
        /// </summary>
        void UpdateElement(DataElement element);

        /// <summary>
        /// 
        /// </summary>
        void DeleteElement(DataElement element);


    }
}
