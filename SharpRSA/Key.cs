using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SharpRSA
{
    /// <summary>
    /// Wrapper KeyPair class, for the case when people 
    /// </summary>
    public class KeyPair
    {
        public Key private_;
        public Key public_;
    }


    /// <summary>
    /// Class to contain RSA key values for public and private keys. All values readonly and protected
    /// after construction, type set on construction.
    /// </summary>
    public class Key
    {
        //Hidden key constants, n and e are public key variables.
        private BigInteger n;
        private int e = Constants.e;

        //Optional null variable D.
        private BigFloat d;

        //Variable for key type.
        public KeyType type;

        //Constructor that sets values once, values then permanently unwriteable.
        public Key(BigInteger n_, KeyType type_, BigFloat d_=null)
        {
            //Catching edge cases for invalid input.
            if (type_==KeyType.PRIVATE && d_==null) { throw new Exception("Constructed as private, but no d value provided."); }

            //Setting values.
            n = n_;
            type = type_;
            d = d_;
        }
    }

    public enum KeyType
    {
        PUBLIC,
        PRIVATE
    }
}
