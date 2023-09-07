    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
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
                    if (File.Exists(filePath))
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(filePath);

                        XmlNodeList carNodes = xmlDoc.SelectNodes("//Car");

                        foreach (XmlNode carNode in carNodes)
                        {
                            string dateStr = carNode.SelectSingleNode("Date").InnerText;

                            if (DateTime.TryParseExact(dateStr, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime date))
                            {
                                ICarRecord carRecord = new CarRecord
                                {
                                    Date = date,
                                    BrandName = carNode.SelectSingleNode("BrandName").InnerText,
                                    Price = int.Parse(carNode.SelectSingleNode("Price").InnerText)
                                };

                                records.Add(carRecord);
                            }
                            else
                            {
                                Console.WriteLine($"Invalid date format in XML: {dateStr}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("XML file does not exist.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error reading XML file: " + ex.Message);
                }

                return records ?? new List<ICarRecord>();
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
                        dateElement.InnerText = carRecord.Date.ToString("dd.MM.yyyy");

                        XmlElement brandNameElement = xmlDoc.CreateElement("BrandName");
                        brandNameElement.InnerText = carRecord.BrandName;

                        XmlElement priceElement = xmlDoc.CreateElement("Price");
                        priceElement.InnerText = carRecord.Price.ToString();

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
