using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public delegate bool TestDelegate();
    public enum TestType
    {
        MATHS_TEST,
        UTILS_TEST,
        RSA_TEST
    }

    public class Test
    {
        //Delegate for calling test bool.
        public TestDelegate testMethod;
        public TestType type;
    }
}
