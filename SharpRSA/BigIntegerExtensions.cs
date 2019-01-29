using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace SharpRSA
{
    public static class BigIntegerExtensions
    {
        public static BigInteger modinv(BigInteger u, BigInteger v)
        {
            BigInteger inv, u1, u3, v1, v3, t1, t3, q;
            BigInteger iter;
            /* Step X1. Initialise */
            u1 = 1;
            u3 = u;
            v1 = 0;
            v3 = v;
            /* Remember odd/even iterations */
            iter = 1;
            /* Step X2. Loop while v3 != 0 */
            while (v3 != 0)
            {
                /* Step X3. Divide and "Subtract" */
                q = u3 / v3;
                t3 = u3 % v3;
                t1 = u1 + q * v1;
                /* Swap */
                u1 = v1; v1 = t1; u3 = v3; v3 = t3;
                iter = -iter;
            }
            /* Make sure u3 = gcd(u,v) == 1 */
            if (u3 != 1)
                return 0;   /* Error: No inverse exists */
                            /* Ensure a positive result */
            if (iter < 0)
                inv = v - u1;
            else
                inv = u1;
            return inv;
        }

        public static bool IsProbablePrime(this BigInteger source, int certainty)
        {
            if (source == 2 || source == 3)
                return true;
            if (source < 2 || source % 2 == 0)
                return false;

            BigInteger d = source - 1;
            int s = 0;

            while (d % 2 == 0)
            {
                d /= 2;
                s += 1;
            }

            // There is no built-in method for generating random BigInteger values.
            // Instead, random BigIntegers are constructed from randomly generated
            // byte arrays of the same length as the source.
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] bytes = new byte[source.ToByteArray().LongLength];
            BigInteger a;

            for (int i = 0; i < certainty; i++)
            {
                do
                {
                    // This may raise an exception in Mono 2.10.8 and earlier.
                    // http://bugzilla.xamarin.com/show_bug.cgi?id=2761
                    rng.GetBytes(bytes);
                    a = new BigInteger(bytes);
                }
                while (a < 2 || a >= source - 2);

                BigInteger x = BigInteger.ModPow(a, d, source);
                if (x == 1 || x == source - 1)
                    continue;

                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, source);
                    if (x == 1)
                        return false;
                    if (x == source - 1)
                        break;
                }

                if (x != source - 1)
                    return false;
            }

            return true;
        }
    }
}
