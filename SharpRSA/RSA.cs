﻿using System;
using System.Collections.Generic;
using System.Numerics;

namespace SharpRSA
{
    public class RSA
    {
        //Generates a keypair of the required bit length, and returns it.
        public static KeyPair GenerateKeyPair(int bitlength)
        {
            //Generating primes, checking if the GCD of (n-1)(p-1) and e is 1.
            BigInteger q,p,n,x,d = new BigInteger();
            do
            {
                q = FindPrime(bitlength / 2);
            } while (q % Constants.e == 1);
            do
            {
                p = FindPrime(bitlength / 2);
            } while (p % Constants.e == 1);

            n = q * p;
            x = (p - 1) * (q - 1);

            //Computing D such that ed = 1%x.
            d = BigIntegerExtensions.modinv(Constants.e, x);

            if ((d*Constants.e)%x!=1) {
                Console.WriteLine("INVALID D VALUE");
                Console.WriteLine(d * Constants.e + " != " + 1 % x);
            }

            //Returning results.
            return KeyPair.Generate(n, d);
        }

        //Finds a prime of the given bit length, to be used as n and p in RSA key calculations.
        public static BigInteger FindPrime(int bitlength)
        {
            //Generating a random number of bit length.
            if (bitlength%8 != 0)
            {
                throw new Exception("Invalid bit length for key given, cannot generate primes.");
            }

            //Filling bytes with pseudorandom.
            byte[] randomBytes = new byte[(bitlength / 8)+1];
            Maths.rand.NextBytes(randomBytes);
            //Making the extra byte 0x0 so the BigInts are unsigned (little endian).
            randomBytes[randomBytes.Length - 1] = 0x0;

            //Setting the bottom bit and top two bits of the number.
            //This ensures the number is odd, and ensures the high bit of N is set when generating keys.
            Utils.SetBitInByte(0, ref randomBytes[0]);
            Utils.SetBitInByte(7, ref randomBytes[randomBytes.Length - 2]);
            Utils.SetBitInByte(6, ref randomBytes[randomBytes.Length - 2]);

            while (true)
            {
                //Performing a Rabin-Miller primality test.
                bool isPrime = Maths.RabinMillerTest(randomBytes, 40);
                if (isPrime)
                {
                    break;
                } else
                {
                    Utils.IncrementByteArrayLE(ref randomBytes, 2);
                    var upper_limit = new byte[randomBytes.Length];

                    //Clearing upper bit for unsigned, creating upper and lower bounds.
                    upper_limit[randomBytes.Length - 1] = 0x0;
                    BigInteger upper_limit_bi = new BigInteger(upper_limit);
                    BigInteger lower_limit = upper_limit_bi - 20;
                    BigInteger current = new BigInteger(randomBytes);

                    if (lower_limit<current && current<upper_limit_bi)
                    {
                        //Failed to find a prime, returning -1.
                        //Reached limit with no solutions.
                        return new BigInteger(-1);
                    }
                }
            }

            //Returning working BigInt.
            return new BigInteger(randomBytes);
        }

        //Encrypts a set of bytes when given a public key.
        public static byte[] EncryptBytes(byte[] bytes, Key public_key)
        {
            //Checking that the size of the bytes is less than n, and greater than 1.
            if (1>=bytes.Length || bytes.Length>=public_key.n)
            {
                throw new Exception("Bytes given are longer than length of key element n (" + bytes.Length + " bytes).");
            }

            //Padding the array to the size of n.
            byte[] bytes_padded = new byte[public_key.n.ToByteArray().Length];
            Array.Copy(bytes, bytes_padded, bytes.Length);
            
            //Setting high byte right before the data, to prevent data loss.
            bytes_padded[bytes.Length] = 0xFF;

            //Computing as a BigInteger the encryption operation.
            var cipher_bigint = new BigInteger();
            var padded_bigint = new BigInteger(bytes_padded);
            cipher_bigint = BigInteger.ModPow(padded_bigint, public_key.e, public_key.n);

            //Returning the byte array of encrypted bytes.
            return cipher_bigint.ToByteArray();
        }

        //Decrypts a set of bytes when given a private key.
        public static byte[] DecryptBytes(byte[] bytes, Key private_key)
        {
            //Checking that the private key is legitimate, and contains d.
            if (private_key.type!=KeyType.PRIVATE)
            {
                throw new Exception("Private key given for decrypt is classified as non-private in instance.");
            }

            //Padding bytes to ensure unsigned int comparison.
            byte[] bytes_padded = new byte[private_key.n.ToByteArray().Length];
            Array.Copy(bytes, bytes_padded, bytes.Length);

            //Decrypting.
            var plain_bigint = new BigInteger();
            var padded_bigint = new BigInteger(bytes_padded);
            plain_bigint = BigInteger.ModPow(padded_bigint, private_key.d, private_key.n);

            //Removing all padding bytes, including the marker 0xFF.
            byte[] plain_bytes = plain_bigint.ToByteArray();
            int lengthToCopy=-1;
            for (int i=plain_bytes.Length-1; i>=0; i--) 
            {
                if (plain_bytes[i]==0xFF)
                {
                    lengthToCopy = i;
                }
            }

            //Checking for a failure to find marker byte.
            if (lengthToCopy==-1)
            {
                throw new Exception("Marker byte for padding (0xFF) not found in plain bytes. Decryption failed?");
            }

            //Copying into return array, returning.
            byte[] return_array = new byte[lengthToCopy];
            Array.Copy(plain_bytes, return_array, lengthToCopy);
            return return_array;
        }
    }
}
