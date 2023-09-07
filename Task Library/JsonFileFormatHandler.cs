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
                        if (IsValidCarRecord(carRecord))
                        {
                            records.Add(carRecord);
                        }
                        else
                        {
                            Console.WriteLine($"Invalid car record: Date={carRecord.Date}, BrandName={carRecord.BrandName}, Price={carRecord.Price}");
                        }
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

        private bool IsValidCarRecord(CarRecord carRecord)
        {
            // Check if the date is valid (non-zero values for day and month, and valid year).
            if (carRecord.Date.Day <= 0 || carRecord.Date.Month <= 0 || carRecord.Date.Year <= 0)
            {
                return false;
            }

            // Check if the price is valid (greater than zero).
            if (carRecord.Price.ToString().StartsWith("-"))
            {
                return false;
            }

            // Check if BrandName is not null and not empty.
            if (string.IsNullOrEmpty(carRecord.BrandName))
            {
                return false;
            }

            return true;
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
            List<ICarRecord> validRecords = records.Where(r => r.Price > 0).ToList();

            if (validRecords.Count > 0)
            {
                string targetFileName = Path.GetFileNameWithoutExtension(sourceFilePath) + " Converted.xml";
                string targetFilePath = Path.Combine(Path.GetDirectoryName(sourceFilePath), targetFileName);

                IFileFormatHandler xmlHandler = new XmlFileFormatHandler();
                xmlHandler.WriteRecords(targetFilePath, validRecords);

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
