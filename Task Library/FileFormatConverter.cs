using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
