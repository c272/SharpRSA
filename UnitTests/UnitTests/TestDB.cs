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
            AddTest(BigIntegerPaddingConsistency, TestType.MATHS_TEST, "BigInt Padding Consistency");
            AddTest(EEModInv, TestType.MATHS_TEST, "Modular Inverse (Extended Euclidian)");

            //Adding RSA unit tests.
            AddTest(FindPrimeSmallbit, TestType.RSA_TEST, "Find Prime Smallbit");
            AddTest(FindPrimeLargebit, TestType.RSA_TEST, "Find Prime Largebit");
            AddTest(RSAKeyGen, TestType.RSA_TEST, "RSA Keygen Validity");
            AddTest(RSACoverageTest, TestType.RSA_TEST, "RSA Coverage Testing");
            AddTest(LockedBytesReliability, TestType.RSA_TEST, "LockedBytes RSA Reliability");
            AddTest(ClassEncryption, TestType.RSA_TEST, "RSA Class Encryption");
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
                bool isPrime = Maths.RabinMillerTest(new BigInteger(prime), 40);
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

        //Testing the Extended Euclidian modular inverse function.
        public bool EEModInv()
        {
            BigInteger result = Maths.ModularInverse(129031, 13);
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
            KeyPair keys = RSA.GenerateKeyPair(1024);

            //Setting up a payload, testing for perfect encrypt/decrypt.
            byte[] package = { 0xFF, 0x2A, 0x00, 0x00, 0x01, 0x00 };
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

        //Testing the encryption and decryption method with a randomly generated long set of 1020 bits.
        public bool RSACoverageTest()
        {
            KeyPair keys = RSA.GenerateKeyPair(1024);

            //Randomly filling a 126 byte array. (leaving some free space, n can sometimes (1%) be a bit smaller than 128 bytes).
            byte[] b = new byte[126];
            Random rand = new Random(Environment.TickCount);
            rand.NextBytes(b);

            //Encrypting and decrypting, testing reliability.
            byte[] encrypted = RSA.EncryptBytes(b, keys.public_);
            byte[] decrypted = RSA.DecryptBytes(encrypted, keys.private_);
            if (decrypted.SequenceEqual(b))
            {
                Console.WriteLine("SUCCESS: Randomized RSA payload encrypted and decrypted successfully.");
                return true;
            } else
            {
                Console.WriteLine("Randomized RSA payload failed to encrypt and decrypt without data loss. Check SharpRSA package for errors.");
                return false;
            }
        }
        
        //Testing the locked bytes class.
        public bool LockedBytesReliability()
        {
            KeyPair test = RSA.GenerateKeyPair(64);
            byte[] b = { 0xFF };

            LockedBytes locked = new LockedBytes(b, test.public_);
            byte[] unlocked = locked.DecryptBytes(test.private_);

            if (b.SequenceEqual(unlocked))
            {
                return true;
            } else
            {
                return false;
            }
        }

        //Testing class encryption in RSA.
        public bool ClassEncryption()
        {
            KeyPair test = RSA.GenerateKeyPair(64);

            var dummy = new DummyClass();

            //Test variable for integrity check.
            dummy.dummy = 1;

            LockedBytes encryptedDummy = RSA.EncryptClass(dummy, test.public_);
            var newdummy = RSA.DecryptClass<DummyClass>(encryptedDummy, test.private_);
            if (newdummy.dummy==dummy.dummy)
            {
                return true;
            } else
            {
                return false;
            }
        }
    }

    //Some additional testing classes.
    [Serializable]
    public class DummyClass
    {
        //A dummy class with a public integer always set to one, for RSA testing.
        public int dummy = 1;
    }
}
