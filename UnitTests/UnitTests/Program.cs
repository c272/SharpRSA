using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpRSA;

namespace UnitTests
{
    class Program
    {
        static void Main(string[] args)
        {
            //Checking for testing arguments.
            if (args.Length!=0)
            {
                switch (args[0])
                {
                    case "--maths_only":
                        //Running only unit tests for maths.
                        break;
                    case "--rsa_only":
                        //Running only unit tests for RSA module.
                        break;
                    case "--utils_only":
                        //Running only utilities tests.
                        break;
                }
            }
        }


    }
}
