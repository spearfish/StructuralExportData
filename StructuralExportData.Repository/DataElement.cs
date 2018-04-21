using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructuralExportData.Repository
{
    public sealed class DataElement : ICloneable
    {
        public bool ReadOnly { get; private set; }

        internal string ElementId { get; set; }

        internal List<DataParameter> Parameters { get; set; }

        public object Clone()
        {
            var clone = new DataElement();
            ReadOnly = false;
            clone.ElementId = this.ElementId;
            clone.Parameters = this.Parameters;

            return clone;
        }
    }
}
