using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using StructuralExportData.Repository;

namespace StructuralExportData.AddIn
{
    class DatabaseExportProgram
    {
        private List<string> _StructuralFramingCompleteParameters { get;  set; }
        private List<string> _StructuralColumnCompleteParameters { get; set; }
        private List<string> _StructuralFoundationCompleteParameters { get; set; }
        private List<string> _SlabCompleteParameters { get; set; }

        //***********************************DatabaseExportProgram***********************************
        internal DatabaseExportProgram(UIApplication uiapp, UIDocument uidoc, Application app, Document doc)
        {
            // Collect model elements
            CollectRevitModelElements modelElements = new CollectRevitModelElements();
            ElementRepository elementRepository = new ElementRepository();
            CSVDataRepository cSVDataRepository = new CSVDataRepository();

            // Create the structural framing database
            List<Element> StructuralFramingElements = modelElements.GetStructuralFramingElements(doc);
            if(StructuralFramingElements != null || StructuralFramingElements.Count > 0)
                CreateDatabase(doc, StructuralFramingElements, elementRepository, cSVDataRepository, "Framing");

            // Create the structural column database 
            List<Element> StructuralColumnElements = modelElements.GetStructuralColumnElements(doc);
            if (StructuralColumnElements != null || StructuralColumnElements.Count > 0)
                CreateDatabase(doc, StructuralColumnElements, elementRepository, cSVDataRepository, "Column");

            // Create the structural foundation database
            List<Element> StructuralFoundationElements = modelElements.GetStructuralFoundationElements(doc);
            if (StructuralFoundationElements != null || StructuralFoundationElements.Count > 0)
                CreateDatabase(doc, StructuralFoundationElements, elementRepository, cSVDataRepository, "Foundation");

            // Create the structural slab database
            List<Element> SlabElements = modelElements.GetSlabElements(doc);
            if (SlabElements != null || SlabElements.Count > 0)
                CreateDatabase(doc, SlabElements, elementRepository, cSVDataRepository, "Slab");

            // Create the structural wall database
            List<Element> WallElements = modelElements.GetWallElements(doc);
            if (WallElements != null || WallElements.Count > 0)
                CreateDatabase(doc, WallElements, elementRepository, cSVDataRepository, "Wall");
        }

        //***********************************CreateDatabase***********************************
        private void CreateDatabase(Document doc, List<Element> Elements,
            ElementRepository elementRepository, CSVDataRepository cSVDataRepository, string type)
        {
            var DictionaryData = GetParameterData(doc, Elements, type);
            List<string> Parameters = GetCompleteParameters(type);

            elementRepository.CreateElement(DictionaryData, Parameters);
            IReadOnlyList<DataElement> dataElement = elementRepository.ReadElement();

            cSVDataRepository.Create(dataElement, type);
        }

        //***********************************GetParameterData***********************************
        private Dictionary<string, List<Tuple<string, string>>> GetParameterData(Document doc, List<Element> elements, string type)
        {
            List<string> CompleteParameters = new List<string>();
            var DictionaryOfData = new Dictionary<string, List<Tuple<string, string>>>();

            foreach (Element e in elements)
            {
                CollectRevitData collectRevitData = new CollectRevitData();
                var data = collectRevitData.GetElementParameterInformation(doc, e);
                DictionaryOfData.Add(e.Id.ToString(), data);

                List<string> ElementParameters = collectRevitData._Parameters;
                foreach (string s in ElementParameters)
                {
                    if (!CompleteParameters.Contains(s))
                        CompleteParameters.Add(s);
                }
            }
            CompleteParameters.Sort();

            SetCompleteParameters(type, CompleteParameters);

            return DictionaryOfData;
        }

        //***********************************SetCompleteParameters***********************************
        private void SetCompleteParameters(string type, List<string> CompleteParameters)
        {
            switch (type)
            {
                case "Framing":
                    _StructuralFramingCompleteParameters = CompleteParameters;
                    break;
                case "Column":
                    _StructuralColumnCompleteParameters = CompleteParameters;
                    break;
                case "Foundation":
                    _StructuralFoundationCompleteParameters = CompleteParameters;
                    break;
                case "Slab":
                    _SlabCompleteParameters = CompleteParameters;
                    break;
            }
        }

        //***********************************GetCompleteParameters***********************************
        private List<string> GetCompleteParameters(string type)
        {
            List<string> parameters = new List<string>();
            switch (type)
            {
                case "Framing":
                    parameters = _StructuralFramingCompleteParameters;
                    break;
                case "Column":
                    parameters = _StructuralColumnCompleteParameters;
                    break;
                case "Foundation":
                    parameters = _StructuralFoundationCompleteParameters;
                    break;
                case "Slab":
                    parameters = _SlabCompleteParameters;
                    break;
            }
            return parameters;
        }
    }
}
