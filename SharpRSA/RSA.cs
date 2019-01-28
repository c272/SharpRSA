using System;
using System.Numerics;

namespace SharpRSA
{
    public class RSA {
    
        /// <summary>
        /// Finds a prime of the given bit length.
        /// Does NOT work on small bitlength values.
        /// </summary>
        /// <param name="bitlength"></param>
        /// <returns></returns>
        public static BigInteger FindPrime(int bitlength)
        {
            //Generating a random number of bit length half of the given parameter.
            if (bitlength%8 != 0)
            {
                throw new Exception("Invalid bit length for key given, cannot generate primes.");
            }

            //Filling bytes with pseudorandom.
            byte[] randomBytes = new byte[bitlength / 8];
            Maths.rand.NextBytes(randomBytes);

            //Setting the bottom bit and top two bits of the number.
            //This ensures the number is odd, and ensures the high bit of N is set when generating keys.
            Utils.SetBitInByte(0, ref randomBytes[randomBytes.Length-1]);
            Utils.SetBitInByte(7, ref randomBytes[0]);
            Utils.SetBitInByte(6, ref randomBytes[0]);

            while (true)
            {
                //Performing a Rabin-Miller primality test.
                bool isPrime = Maths.RabinMillerTest(randomBytes, 20);
                if (isPrime)
                {
                    break;
                } else
                {
                    Utils.IncrementByteArray(ref randomBytes, 2);
                    var b = new BigInteger(randomBytes);

                    //Checking for limit reached.
                    var upper_limit = new byte[randomBytes.Length];
                    Utils.SetToMaxValue(ref upper_limit);
                    var lower_limit = upper_limit;
                    Utils.DecrementByteArray(ref lower_limit, 10);
                    
                    if (b<new BigInteger(upper_limit) && b>new BigInteger(lower_limit))
                    {
                        //Failed to find a prime, returning -1.
                        return new BigInteger(-1);
                    }
                }
            }

            //Returning number.
            return new BigInteger(randomBytes);
        }
    }
}
