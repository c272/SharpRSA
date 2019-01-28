using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using SharpRSA;

namespace UnitTests
{
    public class TestDB
    {
        public List<Test> tests = new List<Test>();
        public TestDB()
        {
            //Adding Maths unit tests.
            AddTest(RabinMillerKnownPrimes, TestType.MATHS_TEST, "Rabin Miller Known Primes");
            AddTest(FindPrimeSmallbit, TestType.RSA_TEST, "Find Prime Smallbit");
            AddTest(FindPrimeLargebit, TestType.RSA_TEST, "Find Prime Largebit");
        }

        public void AddTest(TestDelegate t, TestType type, string n)
        {
            tests.Add(new Test(t, type, n));
        }

        //Unit test methods start here.
        public bool RabinMillerKnownPrimes()
        {
            foreach (var prime in Constants.primes) {
                //Testing if known primes appear as such.
                bool isPrime = Maths.RabinMillerTest(new BigInteger(prime), 10);
                if (!isPrime)
                {
                    Console.WriteLine("DISCREPANCY: Known prime " + prime + " returned false from the RabinMiller test.");
                    return false;
                }
            }
            Console.WriteLine("All known primes detected and correct.");
            return true;
        }

        //Test the "FindPrime" method with a small bit pool in the RSA class.
        public bool FindPrimeSmallbit()
        {
            for (int i=0; i<10; i++)
            {
                BigInteger prime = RSA.FindPrime(24);
                if (prime!=-1)
                {
                    Console.WriteLine("Smallbit Test " + i + ": PASSED, found prime "+prime.ToString()+".");
                    return true;
                } else
                {
                    Console.WriteLine("Smallbit Test " + i + ": FAIL");
                }
            }

            //No primes found in 10 tries, test failed.
            return false;
        }

        //Test the "FindPrime" method with a large bit pool in the RSA class.
        public bool FindPrimeLargebit()
        {
            for (int i = 0; i < 10; i++)
            {
                BigInteger prime = RSA.FindPrime(512);
                if (prime != -1)
                {
                    Console.WriteLine("Largebit Test " + i + ": PASSED, found prime " + prime.ToString().Substring(0, 20) + "...");
                    return true;
                }
                else
                {
                    Console.WriteLine("Largebit Test " + i + ": FAIL");
                }
            }

            //No primes found in 10 tries, test failed.
            return false;
        }
    }
}
