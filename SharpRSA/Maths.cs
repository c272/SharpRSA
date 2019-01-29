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
        public static bool RabinMillerTest(BigInteger n, int accuracy_amt)
        {
            //Zero and one are not prime.
            if (n < 2)
            {
                return false;
            }
            else if (n == 2)
            {
                //Catching for two.
                return true;
            }
            else if (n % 2 == 0)
            {
                //No multiples of two.
                return false;
            }

            //Finding n-1 = 2^k * m.
            long k = 1;
            int m = 1;
            BigFloat previousResult = 0;
            while (true)
            {
                BigFloat n_float = new BigFloat(n);
                BigFloat k_exp = BigFloat.Parse((Math.Pow(2, k)).ToString());

                var result = BigFloat.Divide(n_float, k_exp);
                
                //Checking for a decimal return, breaking if so.
                if (result%1!=0)
                {
                    //Returning to previous K value, which provided an integer.
                    k--;
                    m = int.Parse(previousResult.ToString());
                    break;
                }
                
                //Incrementing and setting variables for next loop.
                previousResult = result;
                k++;
            }

            //Looping k times, performing modulus.
            for (int i = 0; i < accuracy_amt; i++)
            {
                //Picking a random number between 1 and n-2.
                byte[] randBytes = new byte[n.ToByteArray().Length-1];
                rand.NextBytes(randBytes);
                BigInteger offset = new BigInteger(randBytes);

                //Random number a has been selected.
                BigInteger a = n - offset;

                //Performing test operation a^m % n
                var x = BigInteger.Pow(a, m) % n;
                if (x==1 || x==n-1)
                {
                    //Could still be prime, continue.
                    continue;
                }

                //Checking for the possibility of primality after x!=1 and x!=n-1.
                for (int j=0; j<k-1; j++)
                {
                    x = BigInteger.Pow(x, 2) % n;
                    if (x==n-1)
                    {
                        continue;
                    }
                }

                //Not prime, with acc_amt*someconstant certainty.
                return false;
            }

            //Loop has finished without finding a composite, return prime.
            return true;
        }

        //An overload wrapper for the RabinMillerTest which accepts a byte array.
        public static bool RabinMillerTest(byte[] bytes, int acc_amt)
        {
            BigInteger b = new BigInteger(bytes);
            return RabinMillerTest(b, acc_amt);
        }

        /// <summary>
        /// Performs an Extended Euclidian algorithm on parameters a and b, returning d,
        /// such that d = gcd(a,b);
        /// </summary>
        /// <returns>D, such that D = gcd(a,b).</returns>
        public static BigInteger ExtendedEuclidean(BigInteger a, BigInteger b)
        {
            //Modular inverse can be executed using a BigInteger class function.
            return BigInteger.ModPow(a, -1, b);
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

            //Returning.
            return a == 0 ? b : a;
        }
    }
}
