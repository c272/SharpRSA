using System;

namespace SharpRSA
{
    public class SharpRSA
    {
        public Random rand = new Random();
        public long FindPrime(int bitlength)
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
            //..
            return 0;
        }
    }
}
