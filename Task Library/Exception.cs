using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Library
{
    public class InvalidDateFormatException : Exception
    {
        public InvalidDateFormatException(string message) : base(message) { }
    }

    public class InvalidPriceFormatException : Exception
    {
        public InvalidPriceFormatException(string message) : base(message) { }
    }

}