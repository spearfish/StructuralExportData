using System;
using System.IO;
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
        public void Create(IReadOnlyList<DataElement> dataElements, string type)
        {
            // Set the file name and get the output directory
            var fileName = DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss-") + type + "-Revit-Data" + ".csv";
            var outputDir = @"C:\rvtdata\";

            // Create the file using the FileInfo object
            FileInfo file = new FileInfo(outputDir + fileName);

            string filePath = file.DirectoryName;

            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException("filePath");
            FilePath = outputDir + fileName;

            Write(dataElements);
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
        public void Write(IReadOnlyList<DataElement> dataElements)
        {
            List<string> CompleteParameters = GetCompleteParameters(dataElements);

            
            List<List<string>> writeToFile = new List<List<string>>();
            using (var fs = new System.IO.FileStream(FilePath, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite))
            {
                using (var sw = new System.IO.StreamWriter(fs))
                {
                    CsvWriter csvW = new CsvWriter(sw);                  

                    //Add headers to CSV file 
                    List<string> header = new List<string>();
                    header.Add("Element Id");
                    foreach (string h in CompleteParameters)
                    {
                        header.Add(h);
                    }
                    writeToFile.Add(header);

                    //Add parameter values to element row.                    
                    foreach (DataElement e in dataElements)
                    {
                        List<string> parameterData = new List<string>();
                        parameterData.Add(e.ElementId);

                        foreach (string data in CompleteParameters)
                        {
                            var parameter = e.Parameters.Find(d => d.ParameterName == data);
                            try
                            {
                                var value = parameter.ParameterValue;
                                parameterData.Add(value);
                            }
                            catch
                            {
                                parameterData.Add("na");
                            }
                        }
                        writeToFile.Add(parameterData);
                    }

                    //Write rows to csv file. 
                    foreach (List<string> row in writeToFile)
                    {
                        foreach (string col in row)
                        {
                            csvW.WriteField(col);
                        }                        
                        csvW.NextRecord();
                    }
                    csvW.Flush();
                    sw.Dispose();

                }
                fs.Dispose();
                fs.Close();
            }
        }

        //***********************************CompleteParameters***********************************
        public List<string> GetCompleteParameters(IReadOnlyList<DataElement> dataElements)
        {
            List<string> parameters = new List<string>();

            DataElement de = dataElements.First();
            List<DataParameter> dataParameters = de.Parameters;

            foreach (DataParameter p in dataParameters)
            {
                parameters.Add(p.ParameterName);
            }

            parameters.Sort();
            return parameters;
        }
    }
}
