using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class Utils
    {
        //Function to return the string representation of the raw bytes in an array.
        public static string RawByteString(byte[] bytes)
        {
           return BitConverter.ToString(bytes).Replace("-", " ");
        }
    }
}
