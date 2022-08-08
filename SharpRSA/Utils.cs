using System.Numerics;

namespace SharpRSA
{
    class Utils
    {
        /// <summary>
        /// Sets a bit in a given ref byte, using an index from 0-7 from the right.
        /// </summary>
        /// <param name="bitNumFromRight">The index of the bit number from the lesser side of the byte.</param>
        /// <param name="toSet">The referenced byte to set.</param>
        public static void SetBitInByte(int bitNumFromRight, ref byte toSet)
        {
            byte mask = (byte)(1 << bitNumFromRight);
            toSet |= mask;
        }

        /// <summary>
        /// Increments the byte array as a whole, by a given amount. Assumes little endian.
        /// Assumes unsigned randomBytes.
        /// </summary>
        public static void IncrementByteArrayLE(ref byte[] randomBytes, int amt)
        {
            BigInteger n = new BigInteger(randomBytes);
            n += amt;
            randomBytes = n.ToByteArray();
        }

        /// <summary>
        /// Decrements the byte array as a whole, by a given amount. Assumes little endian.
        /// Assumes unsigned randomBytes.
        /// </summary>
        public static void DecrementByteArrayLE(ref byte[] randomBytes, int amt)
        {
            BigInteger n = new BigInteger(randomBytes);
            n -= amt;
            randomBytes = n.ToByteArray();
        }
    }
}
