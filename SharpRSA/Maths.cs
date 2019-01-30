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
        public static bool RabinMillerTest(BigInteger source, int certainty)
        {
            //Filter out basic primes.
            if (source == 2 || source == 3)
            {
                return true;
            }
            //Below 2, and % 0? Not prime.
            if (source < 2 || source % 2 == 0)
            {
                return false;
            }

            //Finding even integer below number.
            BigInteger d = source - 1;
            int s = 0;

            while (d % 2 == 0)
            {
                d /= 2;
                s += 1;
            }

            //Getting a random BigInt using bytes.
            Random rng = new Random(Environment.TickCount);
            byte[] bytes = new byte[source.ToByteArray().LongLength];
            BigInteger a;

            //Looping to check random factors.
            for (int i = 0; i < certainty; i++)
            {
                do
                {
                    //Generating new random bytes to check as a factor.
                    rng.NextBytes(bytes);
                    a = new BigInteger(bytes);
                }
                while (a < 2 || a >= source - 2);

                //Checking for x=1 or x=s-1.
                BigInteger x = BigInteger.ModPow(a, d, source);
                if (x == 1 || x == source - 1)
                {
                    continue;
                }

                //Iterating to check for prime.
                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, source);
                    if (x == 1)
                    {
                        return false;
                    }
                    else if (x == source - 1)
                    {
                        break;
                    }
                }

                if (x != source - 1)
                {
                    return false;
                }
            }

            //All tests have failed to prove composite, so return prime.
            return true;
        }

        //An overload wrapper for the RabinMillerTest which accepts a byte array.
        public static bool RabinMillerTest(byte[] bytes, int acc_amt)
        {
            BigInteger b = new BigInteger(bytes);
            return RabinMillerTest(b, acc_amt);
        }

        /// <summary>
        /// Performs a modular inverse on u and v,
        /// such that d = gcd(u,v);
        /// </summary>
        /// <returns>D, such that D = gcd(u,v).</returns>
        public static BigInteger ModularInverse(BigInteger u, BigInteger v)
        {
            //Declaring new variables on the heap.
            BigInteger inverse, u1, u3, v1, v3, t1, t3, q = new BigInteger();
            //Staying on the stack, quite small, so no need for extra memory time.
            BigInteger iteration;

            //Stating initial variables.
            u1 = 1;
            u3 = u;
            v1 = 0;
            v3 = v;

            //Beginning iteration.
            iteration = 1;
            while (v3 != 0)
            {
                //Divide and sub q, t3 and t1.
                q = u3 / v3;
                t3 = u3 % v3;
                t1 = u1 + q * v1;

                //Swap variables for next pass.
                u1 = v1; v1 = t1; u3 = v3; v3 = t3;
                iteration = -iteration;
            }

            if (u3 != 1)
            {
                //No inverse, return 0.
                return 0;
            }
            else if (iteration < 0)
            {
                inverse = v - u1;
            }
            else
            {
                inverse = u1;
            }

            //Return.
            return inverse;
        }

        /// <summary>
        /// Returns the greatest common denominator of both BigIntegers given.
        /// </summary>
        /// <returns>The GCD of A and B.</returns>
        public static BigInteger GCD(BigInteger a, BigInteger b)
        {
            //Looping until the numbers are zero values.
            while (a != 0 && b != 0)
            {
                if (a > b)
                {
                    a %= b;
                }
                else
                {
                    b %= a;
                }
            }

            //Returning check.
            return a == 0 ? b : a;
        }
    }
}
