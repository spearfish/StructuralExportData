using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace StructuralExportData.AddIn
{
    class CollectRevitModelElements
    {
        //***********************************GetStructuralFraming***********************************
        public List<Element> GetStructuralFraming(Document doc)
        {
            List<Element> structuralFraming = GetElements(doc, BuiltInCategory.OST_StructuralFraming);
            return structuralFraming;
        }

        //***********************************GetFirstElement***********************************
        public List<Element> GetElements(Document doc, BuiltInCategory category)
        {
            List<Element> elements = new List<Element>();
            ElementClassFilter familyInstanceFilter = new ElementClassFilter(typeof(FamilyInstance));
            // Category filter 
            ElementCategoryFilter Categoryfilter = new ElementCategoryFilter(category);
            // Instance filter 
            LogicalAndFilter InstancesFilter = new LogicalAndFilter(familyInstanceFilter, Categoryfilter);

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            // Colletion Array of Elements
            ICollection<Element> Elements = collector.WherePasses(InstancesFilter).ToElements();
            foreach (Element e in Elements)
                elements.Add(e);

            return elements;
        }
    }
}
