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

        //***********************************GetStructuralFramingElements***********************************
        internal List<Element> GetStructuralFramingElements(Document doc)
        {
            List<Element> structuralFraming = GetElements(doc, BuiltInCategory.OST_StructuralFraming);
            return structuralFraming;
        }
        //***********************************GetStructuralColumnElements***********************************
        internal List<Element> GetStructuralColumnElements(Document doc)
        {
            List<Element> structuralColumns = GetElements(doc, BuiltInCategory.OST_StructuralColumns);
            return structuralColumns;
        }
        //***********************************GetStructuralFoundationElements***********************************
        internal List<Element> GetStructuralFoundationElements(Document doc)
        {
            List<Element> structuralFoundation = GetElements(doc, BuiltInCategory.OST_StructuralFoundation);
            return structuralFoundation;
        }
        //***********************************GetSlabElements***********************************
        internal List<Element> GetSlabElements(Document doc)
        {
            List<Element> slabs = new List<Element>();

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ICollection<Element> floors = collector.OfClass(typeof(Floor)).ToElements();

            foreach (Element f in floors)
                slabs.Add(f);

            return slabs;
        }

        //***********************************GetWallElements***********************************
        internal List<Element> GetWallElements(Document doc)
        {
            List<Element> walls = new List<Element>();

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ICollection<Element> Walls = collector.OfClass(typeof(Wall)).ToElements();

            foreach (Wall w in Walls)
                walls.Add(w);

            return walls;
        }

        //***********************************GetFirstElement***********************************
        private List<Element> GetElements(Document doc, BuiltInCategory category)
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
