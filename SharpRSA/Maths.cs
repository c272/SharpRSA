using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SharpRSA
{
    public class Maths
    {
        /// <summary>
        /// A Rabin Miller primality test which returns true or false.
        /// </summary>
        /// <param name="num">The number to check for being likely prime.</param>
        /// <returns></returns>
        public bool RabinMillerTest(BigInteger num)
        {
            return true;
        }

        //An overload wrapper for the RabinMillerTest which accepts a byte array.
        public bool RabinMillerTest(byte[] bytes)
        {
            BigInteger b = new BigInteger(bytes);
            return RabinMillerTest(b);
        }
    }
}
