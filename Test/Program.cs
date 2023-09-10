using System;
using System.Collections.Generic;
using Task_Library;

class Program
{
    static void Main(string[] args)
    {
        string xmlFilePath = "C:\\Users\\Ivan\\OneDrive\\Рабочий стол\\Folders\\Tasks\\Task-Library\\Test\\Testing Files\\test.xml";
        string jsonFilePath = "C:\\Users\\Ivan\\OneDrive\\Рабочий стол\\Folders\\Tasks\\Task-Library\\Test\\Testing Files\\test.json";

        IFileFormatHandler xmlHandler = new XmlFileFormatHandler();
        IFileFormatHandler jsonHandler = new JsonFileFormatHandler();

        Console.WriteLine("Records from XML:");
        List<ICarRecord> recordsFromXml = xmlHandler.ReadRecords(xmlFilePath);
        xmlHandler.DisplayRecords(recordsFromXml);

        xmlHandler.ConvertAndRename(xmlFilePath);

        string convertedJsonFilePath = xmlFilePath.Replace(".xml", " Converted.json");
        Console.WriteLine("\nRecords from Converted JSON:");
        List<ICarRecord> recordsFromConvertedJson = jsonHandler.ReadRecords(convertedJsonFilePath);
        jsonHandler.DisplayRecords(recordsFromConvertedJson);

        Console.WriteLine("\nConverting JSON to XML...");
        jsonHandler.ConvertAndRename(convertedJsonFilePath);

        string convertedXmlFilePath = convertedJsonFilePath.Replace(".json", " Converted.xml");
        Console.WriteLine("\nRecords from Converted XML:");
        List<ICarRecord> recordsFromConvertedXml = xmlHandler.ReadRecords(convertedXmlFilePath);
        xmlHandler.DisplayRecords(recordsFromConvertedXml);

        Console.WriteLine("\nConversion and renaming complete.");
    }
}
