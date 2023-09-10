using System;
using System.Collections.Generic;

namespace Task_Library
{
    public class FileFormatConverter
    {
        public static List<ICarRecord> Convert(IFileFormatHandler sourceHandler, IFileFormatHandler targetHandler, string sourceFilePath, string targetFilePath)
        {
            var records = sourceHandler.ReadRecords(sourceFilePath);
            targetHandler.WriteRecords(targetFilePath, records);
            return records;
        }
    }
}
