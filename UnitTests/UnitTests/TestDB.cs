using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using SharpRSA;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace UnitTests
{
    public class TestDB
    {
        public List<Test> tests = new List<Test>();
        public TestDB()
        {
            //Adding Maths unit tests.
            AddTest(RabinMillerKnownPrimes, TestType.MATHS_TEST, "Rabin Miller Known Primes");

            //Adding RSA unit tests.
            AddTest(FindPrimes, TestType.RSA_TEST, "Find Primes by Rabin Miller");
        }

        //Adds a test to the database.
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
                bool isPrime = Maths.RabinMillerTest(new BigInteger(prime), 10);
                if (!isPrime)
                {
                    Console.WriteLine("DISCREPANCY: Known prime " + prime + " returned false from the RabinMiller test.");
                    return false;
                }
            }
            return true;
        }

        public bool FindPrimes()
        {
            //Generating a random prime, for a small bit length for testing purposes.
            for (int i=0; i<20; i++)
            {
                if (RSA.FindPrime(16)==-1)
                {
                    Console.WriteLine("Find primes pass "+(i+1)+": FAILURE");
                } else
                {
                    Console.WriteLine("Find primes pass "+(i+1)+": SUCCESS");
                    return true;
                }
            }

            //Failed to find one.
            return false;
        }
    }
}
