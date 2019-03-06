# SharpRSA
*An RSA implementation in pure C#, using the BigInt class.*

## Setup
To import SharpRSA, simply open the solution in VS2017 or above, and build. After this is done, add a reference to the generated class library and use the following to import:

```
using System;
using SharpRSA;
```

## Usage
The bindings of the class library are very simple, and are easy to understand. Below is an example of a few various use cases.

**Encrypt an Array of Bytes**
```
//The one parameter in this function is the bit length of the keys, which must be divisible by 8.
Keypair kp = RSA.GenerateKeyPair(1024);

byte[] raw = { 0x00, 0x01, 0x02, 0x03, 0x04 ... }

//These bytes are now encrypted using RSA, of the bitlength specified before.
byte[] encrypted = RSA.EncryptBytes(raw, kp.public_);
byte[] decrypted = RSA.DecryptBytes(encrypted, kp.private_);
```

**Encrypt a Class Instance**
```
Keypair kp = RSA.GenerateKeyPair(1024);
ExampleClass example = new ExampleClass(1, 2, 3, 4 ...);

//The class instance is now encrypted with RSA.
LockedBytes encrypted = RSA.EncryptClass(example, kp.public_);
ExampleClass decrypted = RSA.DecryptClass(encrypted, kp.private_);
```

## Networking
You can send public keys and LockedBytes over WCF (Windows Communication Foundation) or other networked means, as it is set up as a DataContract object.

Private keys *cannot* be passed over, as they are set to private, readonly and are not part of the DataContract specified. This is to prevent incorrect asymmetric key usage.



