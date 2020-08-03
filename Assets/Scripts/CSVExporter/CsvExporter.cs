using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace DefaultNamespace.CSVExporter
{
    public class CsvExporter : MonoBehaviour
    {
        private readonly List<string[]> _csvData = new List<string[]>();

        public void ExportCsv()
        {
            SaveFile();
        }
    
        private void SaveFile()
        {
            var metaKeys = new string[]
            {
                "UNIQUE_CODE",
                "COLLECTION_NAME",
                "ARTWORK_DETAILS",
                "ITEM_SKU",
                "ITEM_CATEGORY",
                "ITEM_OPTION",
                "ITEM_UPC",
                "ITEM_PRICE",
                "TIMESTAMP"
            };

            var rowDataTemp = new string[9];
            rowDataTemp[0] = "UniqueCode";
            rowDataTemp[1] = "Customer Name";
            rowDataTemp[2] = "Artwork Details";
            rowDataTemp[3] = "Item SKU";
            rowDataTemp[4] = "Item Category";
            rowDataTemp[5] = "Item Option";
            rowDataTemp[6] = "Item UPC";
            rowDataTemp[7] = "Item Price";
            rowDataTemp[8] = "Timestamp";
            _csvData.Add(rowDataTemp);

            var metaFiles = Directory.GetFiles(OrderAnalysis.SavesPath + @"Meta");
            
            for (int i = 0; i < metaFiles.Length; i++)
            {
                var metaData = XmlReader.ExtractXmlData(metaFiles[i]);
                rowDataTemp = new string[9];
                for (int j = 0; j < metaKeys.Length; j++)
                {
                    var value = metaData[metaKeys[j]];
                    rowDataTemp[j] = value;
                }
                _csvData.Add(rowDataTemp);
            }

            var output = new string[_csvData.Count][];

            for(int i = 0; i < output.Length; i++)
            {
                output[i] = _csvData[i];
            }

            var length= output.GetLength(0);
            var delimiter = ",";

            var sb = new StringBuilder();
        
            for (int index = 0; index < length; index++)
                sb.AppendLine(string.Join(delimiter, output[index]));

            var filePath = GetPath();

            var outStream = File.CreateText(filePath);
            outStream.WriteLine(sb);
            outStream.Close();
        }
        
        private string GetPath()
        {
            return @"C:\YR\CSV\" + "Saved_data.csv";
        }
    }
}