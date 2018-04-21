using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace StructuralExportData.Repository
{
    public class CSVDataRepository : IDatabaseRepository
    {
        public string FilePath { get; private set; }

        //***********************************Create***********************************
        public CSVDataRepository(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException("filePath");
            FilePath = filePath;
        }

        //***********************************Create***********************************
        public void Create(IReadOnlyList<DataElement> dataElements)
        {
            using (var fs = new System.IO.FileStream(FilePath, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite))
            {
                using (var sw = new System.IO.StreamWriter(fs))
                {
                    CsvWriter csvW = new CsvWriter(sw);

                    //IReadOnlyList<Element> elements = _elementRepository.ReadElement;
                    List<string> writeToFile = new List<string>();
                    StringBuilder sb = new StringBuilder();

                    foreach (DataElement e in dataElements)
                    {
                        writeToFile.Add("Element, " + e.ElementId);
                        List<DataParameter> parameters = e.Parameters;
                        //parameters.Sort();
                        foreach (DataParameter p in parameters)
                        {
                            // writeToFile.Add(p.ParameterValue);
                            sb.Append("\n" + p.ParameterName + ", " + p.ParameterValue);
                        }
                        writeToFile.Add(sb.ToString());
                    }

                    foreach (string s in writeToFile)
                    {
                        csvW.WriteField(s);
                        csvW.NextRecord();
                    }


                    sw.Dispose();
                    //csvW.WriteRecord(elements);
                }
                fs.Dispose();
                fs.Close();
            }
        }


        //***********************************Delete***********************************
        public void Delete()
        {
            throw new NotImplementedException();
        }

        //***********************************Read***********************************
        public void Read()
        {
            throw new NotImplementedException();
        }

        //***********************************Update***********************************
        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
