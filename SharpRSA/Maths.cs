using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SharpRSA
{
    public class Maths
    {
        public static Random rand = new Random(Environment.TickCount);

        /// <summary>
        /// A Rabin Miller primality test which returns true or false.
        /// </summary>
        /// <param name="num">The number to check for being likely prime.</param>
        /// <returns></returns>
        public static bool RabinMillerTest(ulong n)
        {
            //Zero and one are not prime.
            if (n<2)
            {
                return false;
            } else if (n%2==0)
            {
                //No multiples of two.
                return false;
            }

            //Finding n-1 = 2^k * m.
            long k = 1;
            long m = 1;
            double previousResult = 0;
            while (true)
            {
                double result = (n - 1) / (Math.Pow(2, k));
                
                //Checking for a decimal return, breaking if so.
                if (result%1!=0)
                {
                    //Returning to previous K value, which provided an integer.
                    k--;
                    m = (long)previousResult;
                    break;
                }
                
                //Incrementing and setting variables for next loop.
                previousResult = result;
                k++;

                //Checking if we should just return false due to the loop being too large.
                if (k>9999999999999999)
                {
                    Console.WriteLine("Loop cancelled due to infinite looping of k value.");
                    return false;
                }
            }

            //Picking a random number between 1 and n-1.
            byte[] randBytes = new byte[63];
            rand.NextBytes(randBytes);
            

            return true;
        }

        //An overload wrapper for the RabinMillerTest which accepts a byte array.
        public static bool RabinMillerTest(byte[] bytes)
        {
            ulong b = BitConverter.ToUInt64(bytes, 0);
            return RabinMillerTest(b);
        }
    }
}
