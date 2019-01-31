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
                        RunTests(TestType.MATHS_TEST);
                        break;
                    case "--rsa_only":
                        //Running only unit tests for RSA module.
                        RunTests(TestType.RSA_TEST);
                        break;
                    case "--utils_only":
                        //Running only utilities tests.
                        RunTests(TestType.UTILS_TEST);
                        break;
                    default:
                        Console.WriteLine("Unrecognised parameter, running all tests.");
                        RunTests();
                        break;
                }
            } else
            {
                //No parameters, running all tests.
                RunTests();
            }
        }

        //Runs all tests, returning results.
        static List<Test> failed = new List<Test>();
        static List<Test> passed = new List<Test>();
        private static void RunTests(TestType filterType=TestType.ALL)
        {
            //Creating test database.
            TestDB tdb = new TestDB();
            //Resetting variables.
            passed = new List<Test>();
            failed = new List<Test>();

            //Adding prepending test data to console.
            Console.WriteLine("SHARPRSA UNIT TEST ENVIRONMENT");
            Console.WriteLine("Machine: " + Environment.MachineName);
            Console.WriteLine("OS: " + Environment.OSVersion);
            Console.WriteLine("Framework Version: " + Environment.Version);
            Console.WriteLine("Start Time: " + DateTime.Now);
            Console.WriteLine("\nBeginning test run, filter type \"" + filterType.ToString() + "\".\n--------------------------\n");

            //Running all tests?
            if (filterType==TestType.ALL)
            {
                foreach (var test in tdb.tests)
                {
                    CheckTest(test);
                }
            } else
            {
                //Running tests for a specific type.
                foreach (var test in tdb.tests)
                {
                    if (test.type==filterType)
                    {
                        CheckTest(test);
                    }
                }
            }

            //Switching for % statistics colour.
            Console.Write("\nPercentage of Passing Tests: ");
            float percentagePassed = ((float)passed.Count / (float)(passed.Count + failed.Count)) * 100;
            if (percentagePassed>90)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            } else if (percentagePassed>70)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            } else if (percentagePassed>50)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            } else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
            }

            //Printing statistics.
            Console.Write(percentagePassed + "%\n");
            Console.ResetColor();
            Console.WriteLine("--------------\nTESTS COMPLETED: {0}\nTESTS PASSED: {1}\nTESTS FAILED: {2}\n--------------", failed.Count + passed.Count, passed.Count, failed.Count);
            if (failed.Count != 0)
            {
                Console.WriteLine("Tests Failed:\n");
                foreach (var f in failed)
                {
                    Console.WriteLine(failed.IndexOf(f) + " - " + f.name);
                }
            }
        }

        public static void CheckTest(Test test)
        {
            bool result = test.testMethod();
            if (!result)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("TEST FAILED! Test \"" + test.name + "\" returned a negative result.");
                Console.ResetColor();
                failed.Add(test);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(test.name + " test passed.");
                Console.ResetColor();
                passed.Add(test);
            }
        }
    }
}
