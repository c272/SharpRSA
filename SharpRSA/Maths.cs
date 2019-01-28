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
        public static BigFloat ExtendedEuclidean(BigFloat a, BigFloat b)
        {
            //Local variables.
            var d = new BigFloat();
            BigFloat x, y, x1, x2, y1, y2, q, r = new BigFloat();

            //Sanity checks to make sure no invalid inputs are passed.
            if (a<b) { throw new Exception("A cannot be smaller than B when computing an extended euclidian."); }
            if (a<0 || b<0) { throw new Exception("Neither values can be negative when computing an extended euclidian."); }

            //Checking for b=0, if so, end right there, d is a.
            if (b==0)
            {
                d = a;
                return d;
            }

            //Non-zero, loop while b>0 and compute.
            x1 = 0;
            x2 = 1;
            y1 = 1;
            y2 = 0;
            while (b>0)
            {
                //Finding R value as a-floor(a/b)*b.
                q = BigFloat.Floor(a / b);
                r = a - q * b;

                //Setting X and Y values by (component2)-floor(a/b)*(component1)
                x = x2 - q * x1;
                y = y2 - q * y1;
                a = b; b = r;

                //Setting x and y values for next loop.
                x2 = x1;
                x1 = x;
                y2 = y1;
                y1 = y;
            }

            //Loop finished, so d=a, return.
            d = a;
            return d;
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
