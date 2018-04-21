using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StructuralExportData.Repository
{
    public class ElementRepository : IElementRepository
    {
        private IList<DataElement> _Elements;

        //***********************************CreateElement***********************************
        public void CreateElement(Dictionary<string, List<Tuple<string, string>>> dictionary, List<string> CompleteParameters)
        {
            List<DataElement> Elements = new List<DataElement>();

            foreach (KeyValuePair<string, List<Tuple<string, string>>> entry in dictionary)
            {
                List<DataParameter> dataParameters = new List<DataParameter>();
                DataElement e = new DataElement();
                e.ElementId = entry.Key;

                List<Tuple<string, string>> listTuple = new List<Tuple<string, string>>();
                listTuple = entry.Value;

                foreach (Tuple<string, string> tuple in listTuple)
                {
                    try
                    {
                        DataParameter p = new DataParameter();
                        p.ParameterName = tuple.Item1;
                        p.ParameterValue = "";
                        if (tuple.Item2 != null)
                            p.ParameterValue = tuple.Item2;

                        dataParameters.Add(p);
                        CompleteParameters.Remove(tuple.Item1);
                    }
                    catch (Exception ex)
                    { MessageBox.Show(ex.Message); }
                }

                // If element doesn't have the parameter it is set to none. 
                if (CompleteParameters != null || CompleteParameters.Count > 0)
                {
                    foreach (string parameter in CompleteParameters)
                    {
                        DataParameter p = new DataParameter();
                        p.ParameterName = parameter;
                        p.ParameterValue = "na";
                        dataParameters.Add(p);
                    }
                }

                e.Parameters = dataParameters;
                Elements.Add(e);
            }

            _Elements = Elements.Select(r => r.Clone() as DataElement).ToList();
        }

        //***********************************DeleteElement***********************************
        public void DeleteElement(DataElement element)
        {
            _Elements.Remove(element);
        }

        //***********************************ReadElement***********************************
        public IReadOnlyList<DataElement> ReadElement()
        {
            return _Elements.Select(r => r.Clone() as DataElement).ToList().AsReadOnly();
        }

        //***********************************UpdateElement***********************************
        public void UpdateElement(DataElement element)
        {
            DataElement e = _Elements.FirstOrDefault(x => x.ElementId == element.ElementId);
            if (e != null)
            {
                var position = _Elements.IndexOf(e);
                _Elements.Remove(e);
                _Elements.Insert(position, element);
            }
        }
    }
}
