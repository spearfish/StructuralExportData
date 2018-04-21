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
        //***********************************Program***********************************
        public DatabaseExportProgram(UIApplication uiapp, UIDocument uidoc, Application app, Document doc)
        {
            var DictionaryOfData = new Dictionary<string, List<Tuple<string, string>>>();
            List<string> CompleteParameters = new List<string>();

            CollectRevitModelElements modelElements = new CollectRevitModelElements();
            //Structural framing elements
            List<Element> StructuralFramingElements = modelElements.GetStructuralFraming(doc);
            List<string> parameters = new List<string>();
            foreach (Element e in StructuralFramingElements)
            {
                CollectRevitData collectRevitData = new CollectRevitData();
                var data = collectRevitData.GetElementParameterInformation(doc, e);
                DictionaryOfData.Add(e.Id.ToString(), data);
                List<string> ElementParameters = collectRevitData._Parameters;
                foreach(string s in ElementParameters)
                {
                    if (!parameters.Contains(s))
                        parameters.Add(s);
                }
            }

            ElementRepository elementRepository = new ElementRepository();
            elementRepository.CreateElement(DictionaryOfData, parameters);
            IReadOnlyList<DataElement> dataElement = elementRepository.ReadElement();

            CSVDataRepository cSVDataRepository = new CSVDataRepository(@"C:\Test.csv");
            cSVDataRepository.Create(dataElement);
        }
    }
}
