using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Task_Library
{
    public class XmlFileFormatHandler : IFileFormatHandler
    {
        public List<ICarRecord> ReadRecords(string filePath)
        {
            List<ICarRecord> records = new List<ICarRecord>();

            try
            {
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("XML file does not exist.", filePath);
                }

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);

                XmlNodeList carNodes = xmlDoc.SelectNodes("//Car");

                foreach (XmlNode carNode in carNodes)
                {
                    string dateStr = carNode.SelectSingleNode("Date")?.InnerText;
                    string brandName = carNode.SelectSingleNode("BrandName")?.InnerText;
                    string priceStr = carNode.SelectSingleNode("Price")?.InnerText;

                    if (string.IsNullOrWhiteSpace(dateStr) || string.IsNullOrWhiteSpace(brandName) || string.IsNullOrWhiteSpace(priceStr))
                    {
                        throw new InvalidOperationException("Invalid XML format - missing or empty elements.");
                    }                 

                    records.Add(new CarRecord
                    {
                        Date = dateStr,
                        BrandName = brandName,
                        Price = priceStr
                    });
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Error reading XML file: " + ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Error reading XML file - Invalid XML format: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading XML file: " + ex.Message);
            }

            return records;
        }

        public void WriteRecords(string filePath, List<ICarRecord> records)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlElement rootElement = xmlDoc.CreateElement("Document");
                xmlDoc.AppendChild(xmlDeclaration);
                xmlDoc.AppendChild(rootElement);

                foreach (var carRecord in records)
                {
                    XmlElement carElement = xmlDoc.CreateElement("Car");

                    XmlElement dateElement = xmlDoc.CreateElement("Date");
                    dateElement.InnerText = carRecord.Date;

                    XmlElement brandNameElement = xmlDoc.CreateElement("BrandName");
                    brandNameElement.InnerText = carRecord.BrandName;

                    XmlElement priceElement = xmlDoc.CreateElement("Price");
                    priceElement.InnerText = carRecord.Price;

                    carElement.AppendChild(dateElement);
                    carElement.AppendChild(brandNameElement);
                    carElement.AppendChild(priceElement);

                    rootElement.AppendChild(carElement);
                }

                xmlDoc.Save(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error writing XML file: " + ex.Message);
            }
        }

        public void ConvertAndRename(string sourceFilePath)
        {
            List<ICarRecord> records = ReadRecords(sourceFilePath);

            if (records.Count > 0)
            {
                string targetFileName = Path.GetFileNameWithoutExtension(sourceFilePath) + " Converted.json";
                string targetFilePath = Path.Combine(Path.GetDirectoryName(sourceFilePath), targetFileName);

                IFileFormatHandler jsonHandler = new JsonFileFormatHandler();
                jsonHandler.WriteRecords(targetFilePath, records);

                Console.WriteLine($"XML to JSON conversion complete. Renamed to: {targetFilePath}");
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
