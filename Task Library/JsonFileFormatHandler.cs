using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;

namespace Task_Library
{
    public class JsonFileFormatHandler : IFileFormatHandler
    {
        public List<ICarRecord> ReadRecords(string filePath)
        {
            List<ICarRecord> records = new List<ICarRecord>();

            try
            {
                if (File.Exists(filePath))
                {
                    string jsonContent = File.ReadAllText(filePath);

                    var settings = new JsonSerializerSettings
                    {
                        DateFormatString = "dd.MM.yyyy",
                    };

                    var carRecords = JsonConvert.DeserializeObject<List<CarRecord>>(jsonContent, settings);

                    foreach (var carRecord in carRecords)
                    {
                        records.Add(carRecord);
                    }
                }
                else
                {
                    Console.WriteLine("JSON file does not exist.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading JSON file: " + ex.Message);
            }

            return records;
        }

        public void WriteRecords(string filePath, List<ICarRecord> records)
        {
            try
            {
                string jsonContent = JsonConvert.SerializeObject(records, Formatting.Indented);

                File.WriteAllText(filePath, jsonContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error writing JSON file: " + ex.Message);
            }
        }

        public void ConvertAndRename(string sourceFilePath)
        {
            List<ICarRecord> records = ReadRecords(sourceFilePath);

            if (records.Count > 0)
            {
                string targetFileName = Path.GetFileNameWithoutExtension(sourceFilePath) + " Converted.xml";
                string targetFilePath = Path.Combine(Path.GetDirectoryName(sourceFilePath), targetFileName);

                IFileFormatHandler xmlHandler = new XmlFileFormatHandler();
                xmlHandler.WriteRecords(targetFilePath, records);

                Console.WriteLine($"JSON to XML conversion complete. Renamed to: {targetFilePath}");
            }
            else
            {
                Console.WriteLine($"No valid records found in {sourceFilePath}. Conversion skipped.");
            }
        }

        public void DisplayRecords(List<ICarRecord> records)
        {
            foreach (var record in records)
            {
                Console.WriteLine($"Date: {record.Date}, BrandName: {record.BrandName}, Price: {record.Price}");
            }
        }
    }
}
