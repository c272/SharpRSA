using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SharpRSA
{
    public class LockedBytes
    {
        /// <summary>
        /// Parses, chunks and encrypts the bytes, storing as a list of encrypted byte arrays.
        /// </summary>
        /// <param name="b">The bytes to encrypt.</param>
        /// <param name="public_">The public key to encrypt with.</param>
        public LockedBytes(byte[] b, Key public_)
        {
            //Getting the max chunk length (public key byte length -2) and amount of chunks in the given byte array b.
            initialByteLength = b.Length;
            maxChunkLength = public_.n.ToByteArray().Length - 2;
            chunkModulus = b.Length % maxChunkLength;
            chunks = (int)(Math.Ceiling((double)(b.Length/maxChunkLength))+0.5);
            Console.WriteLine("chunks: " + chunks);

            for (int i=0; i<chunks; i++)
            {
                //Copying the selected part of array b to the new chunk created.
                byte[] unencrypted = new byte[maxChunkLength];
                Array.Copy(b, i * maxChunkLength, unencrypted, 0, maxChunkLength);

                //Encrypting that chunk.
                byte[] encrypted = new byte[maxChunkLength];
                encrypted = RSA.EncryptBytes(unencrypted, public_);

                //Adding to list.
                byteChunks.Add(encrypted);
            }
        }

        /// <summary>
        /// Decrypts the bytes held within the class using the given private key.
        /// </summary>
        /// <param name="private_">The private key to decrypt with.</param>
        /// <returns></returns>
        public byte[] DecryptBytes(Key private_)
        {
            Console.WriteLine("byteChunks Length: " + byteChunks.Count);
            Console.WriteLine("Chunks: " + chunks);
            Console.WriteLine("Chunk Size: " + maxChunkLength);
            Console.WriteLine("Chunk Modulus: " + chunkModulus);

            //Creating a list of decrypted padded bytes.
            List<byte[]> decryptedList = new List<byte[]>();
            for (int i=0; i<chunks; i++)
            {
                decryptedList.Add(RSA.DecryptBytes(byteChunks[i], private_));
            }

            //Removing any extra padding on the last byte array.
            if (chunkModulus != 0)
            {
                byte[] nopadding = new byte[chunkModulus];
                Array.Copy(decryptedList[decryptedList.Count - 1], nopadding, chunkModulus);
                decryptedList[decryptedList.Count - 1] = nopadding;
            }

            //Converting back to a single byte array, returning.
            return decryptedList.SelectMany(a => a).ToArray();
        }

        private int chunkModulus;
        private int initialByteLength;
        private int chunks;
        private int maxChunkLength;
        private List<byte[]> byteChunks = new List<byte[]>();
    }
}
