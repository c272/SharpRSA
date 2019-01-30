using System;
using System.Collections.Generic;
using System.Text;

namespace SharpRSA
{
    public class EncryptedBytes<T>
    {
        //Holds an array of encrypted byte arrays, all keybytelength-2 bytes long (final one has no padding).
        public List<byte[]> bytes;
        public T type;

        //Constructor.
        public EncryptedBytes()
        {

        }
    }
}
