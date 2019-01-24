using System;
using System.Numerics;

namespace SharpRSA
{
    public class SharpRSA
    {
        public BigInteger FindPrime(int bitlength)
        {
            //Generating a random number of bit length half of the given parameter.
            bitlength = bitlength / 2;
            if (bitlength%8 != 0)
            {
                throw new Exception("Invalid bit length for key given, cannot generate primes.");
            }

            //Filling bytes with pseudorandom.
            byte[] randomBytes = new byte[bitlength / 8];
            Maths.rand.NextBytes(randomBytes);

            //Setting the bottom bit and top two bits of the number.
            //This ensures the number is odd, and ensures the high bit of N is set when generating keys.
            Utils.SetBitInByte(0, ref randomBytes[7]);

            while (true)
            {
                //Performing a Rabin-Miller primality test.
                bool isPrime = Maths.RabinMillerTest(randomBytes, 20);
                if (isPrime)
                {
                    break;
                } else
                {
                    Utils.IncrementByteArray(ref randomBytes);
                }
            
            }

            //placeholder
            return new BigInteger();
        }
    }
}
