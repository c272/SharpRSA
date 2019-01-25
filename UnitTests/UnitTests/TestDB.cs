using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace UnitTests
{
    public class TestDB
    {
        public List<Test> tests = new List<Test>();
        public TestDB()
        {
            //Adding Maths unit tests.
            AddTest(RabinMillerKnownPrimes, TestType.MATHS_TEST, "Rabin Miller Known Primes");
        }

        public void AddTest(TestDelegate t, TestType type, string n)
        {
            tests.Add(new Test(t, type, n));
        }

        //Unit test methods start here.
        public bool RabinMillerKnownPrimes()
        {
            int[] primes = { 2, 5, 7, 11, 13, 15485867, 32452867, 982451653 };
            foreach (var prime in primes) {
                //Testing if known primes appear as such.
                bool isPrime = SharpRSA.Maths.RabinMillerTest(new BigInteger(prime), 10);
                if (!isPrime)
                {
                    Console.WriteLine("DISCREPANCY: Known prime " + prime + " returned false from the RabinMiller test.");
                    return false;
                }
            }
            return true;
        }
    }
}
