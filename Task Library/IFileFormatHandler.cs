using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Library
{
    public interface IFileFormatHandler
    {
        List<ICarRecord> ReadRecords(string filePath);
        void WriteRecords(string filePath, List<ICarRecord> records);
        void ConvertAndRename(string sourceFilePath);
        void DisplayRecords(List<ICarRecord> records);
    }

}
