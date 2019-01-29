﻿using System;
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
            AddTest(BigIntegerPaddingConsistency, TestType.MATHS_TEST, "BigInt Padding Consistency");
            AddTest(EEModInv, TestType.MATHS_TEST, "Modular Inverse (Extended Euclidian)");

            //Adding RSA unit tests.
            AddTest(FindPrimeSmallbit, TestType.RSA_TEST, "Find Prime Smallbit");
            AddTest(FindPrimeLargebit, TestType.RSA_TEST, "Find Prime Largebit");
            AddTest(RSAKeyGen, TestType.RSA_TEST, "RSA Keygen Validity");
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

        //Testing the BigInteger class for a reliable byte padding method.
        public bool BigIntegerPaddingConsistency()
        {
            byte[] b = { 0x01, 0x00, 0xFF, 0x00, 0x00, 0x00 };
            byte[] nopad = { 0x01, 0x00, 0xFF };
            byte[] singlepad = { 0x01, 0x00, 0xFF, 0x00 };
            var big = new BigInteger(b).ToByteArray();

            if (big.SequenceEqual(nopad))
            {
                Console.WriteLine("BigInteger is using no padding, converted and returned unsigned are identical.");
                return true;
            } else if (big.SequenceEqual(singlepad))
            {
                Console.WriteLine("BigInteger is using single padding, one extra blank byte appended for unsigned.");
                return true;
            }
            {
                Console.WriteLine("The converted and non-converted data differ as such:");
                Console.WriteLine("ORIGINAL: " + Utils.RawByteString(b));
                Console.WriteLine("CONVERTED: " + Utils.RawByteString(big));
                Console.WriteLine("No pattern detected.");
                return false;
            }
            
        }

        //Testing the encryption and decryption methods to check small byte packages.
        public bool RSASmallbytes()
        {
            byte[] package = { 0xFF, 0x2A, 0xBF, 0x00, 0x00 };
            return false;
            
        }

        //Testing the Extended Euclidian modular inverse function.
        public bool EEModInv()
        {
            BigInteger result = Maths.ExtendedEuclidean(129031, 13);
            if (result!=11)
            {
                return false;
            }
            return true;
        }

        //Testing the key generation method to check valid keys are returned.
        public bool RSAKeyGen()
        {
            //Generating a test 1024 bit keypair.
            KeyPair keys = RSA.GenerateKeyPair(64);

            //Setting up a payload, testing for perfect encrypt/decrypt.
            byte[] package = { 0xFF, 0x2A, 0xBF, 0x00, 0x00 };
            byte[] encrypted = RSA.EncryptBytes(package, keys.public_);
            byte[] decrypted = RSA.DecryptBytes(encrypted, keys.private_);

            //Checking decrypt.
            if (decrypted.SequenceEqual(package))
            {
                return true;
            } else
            {
                Console.WriteLine("Returned bytes from RSA encrypt/decrypt differed, resulting in failure.");
                Console.WriteLine("ORIGINAL BYTES: " + Utils.RawByteString(package));
                Console.WriteLine("FAILED BYTES:" + Utils.RawByteString(decrypted));
                return false;
            }
        }
    }
}
