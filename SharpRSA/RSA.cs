using System;

namespace SharpRSA
{
    public class SharpRSA
    {
        public Random rand = new Random();
        public byte[] FindPrime(int bitlength)
        {
            //Generating a random number of bit length half of the given parameter.
            bitlength = bitlength / 2;
            if (bitlength%8 != 0)
            {
                throw new Exception("Invalid bit length for key given, cannot generate primes.");
            }

            //Filling bytes with pseudorandom.
            byte[] randomBytes = new byte[bitlength / 8];
            rand.NextBytes(randomBytes);

            //Setting the bottom bit and top two bits of the number.
            //This ensures the number is odd, and ensures the high bit of N is set when generating keys.
            Utils.SetBitInByte(0, ref randomBytes[7]);

            //Performing a Rabin-Miller primality test.
            

            //placeholder
            return new byte[8];
        }
    }
}
