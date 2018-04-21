using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace StructuralExportData.AddIn
{
    class CollectRevitData
    {
        public List<string> _Parameters { get; set; }

        //***********************************GetElementParameterInformation***********************************
        public List<Tuple<string, string>> GetElementParameterInformation(Document doc, Element element)
        {
            List<string> Parameters = new List<string>();
            String prompt = "Show parameters";

            var data = new List<Tuple<string, string>>();
            StringBuilder st = new StringBuilder();

            foreach (Parameter param in element.Parameters)
            {
                Tuple<string, string> NameAndData = GetParameterInformation(doc, param);

                data.Add(new Tuple<string, string>(NameAndData.Item1, NameAndData.Item2));

                st.AppendLine(NameAndData.Item1 + "\t : " + NameAndData.Item2);
                //Add all parameters to a list
                if (!Parameters.Contains(NameAndData.Item1))
                    Parameters.Add(NameAndData.Item1);
            }

            TaskDialog.Show("Revit", prompt + st.ToString());
            _Parameters = Parameters;
            return data;
        }

        //***********************************GetParameterInformation***********************************
        private Tuple<string, string> GetParameterInformation(Document document, Parameter para)
        {
            string defName = para.Definition.Name;
            string defValue = string.Empty;
            // Use different method to get parameter data according to the storage type
            switch (para.StorageType)
            {
                case StorageType.Double:
                    //covert the number into Metric
                    defValue = para.AsValueString();
                    break;
                case StorageType.ElementId:
                    //find out the name of the element
                    Autodesk.Revit.DB.ElementId id = para.AsElementId();
                    if (id.IntegerValue >= 0)
                    {
                        defValue = document.GetElement(id).Name;
                    }
                    else
                    {
                        defValue = id.IntegerValue.ToString();
                    }
                    break;
                case StorageType.Integer:
                    if (ParameterType.YesNo == para.Definition.ParameterType)
                    {
                        if (para.AsInteger() == 0)
                        {
                            defValue = "False";
                        }
                        else
                        {
                            defValue = "True";
                        }
                    }
                    else
                    {
                        defValue = para.AsInteger().ToString();
                    }
                    break;
                case StorageType.String:
                    defValue = para.AsString();
                    break;
                default:
                    defValue = "Unexposed parameter.";
                    break;
            }

            return Tuple.Create(defName, defValue);
        }
    }
}
