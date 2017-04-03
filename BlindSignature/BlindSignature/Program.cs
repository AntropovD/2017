using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using BlindSignature;

namespace BlindSignature
{
    class Program
    {
        const string keyFile = "key.txt";


	    static void Main()
	    {
		    var array = new int[] {1, 2, 3, 4, 5};
			var rotator = new TemplateRotator();
			
			Console.WriteLine(string.Join(" ", rotator.RotateRight(array, 1)));
			Console.WriteLine(string.Join(" ", rotator.RotateLeft(array, 1)));


		}
//        static void Main(string[] args)
//        {
//            var blindSignature = new BlindSignature();
//            blindSignature.Run(keyFile);
//        }

        public class BlindSignature
        {
            private BigInteger secretKey;
            private BigInteger publicKey;
            private BigInteger P;

            private BigInteger K;            

            public void Run(string filename)
            {
                var data = File.ReadAllLines(filename);
                if (data.Length != 3)
                {
                    Console.WriteLine("Wrong input data");
                    return;
                }
                ParseData(data);             
                Console.WriteLine("Enter message: ");

                var message = Console.ReadLine().ToCharArray().Select(c => (BigInteger)c); ;
                Console.WriteLine("Your message numbers:");
                Console.WriteLine(Join(message));

                Console.WriteLine("Alice sends Bob masked message");
                var maskedMessage = MaskMessage(message);
                Console.WriteLine(Join(maskedMessage));

                Console.WriteLine("Bob signs message and sends back to Alice");
                var signedMessage = SignMessage(maskedMessage);
                Console.WriteLine(Join(signedMessage));

                Console.WriteLine("Alice receives signed message and unmasks it");
                var unmaskedMessage = UnmaskMessage(signedMessage);
                Console.WriteLine(Join(unmaskedMessage));
            }

            public string Join(IEnumerable<BigInteger> data)
            {
                return string.Join(" ", data.Select(m => m.ToString()));
            }

            public void ParseData(string[] data)
            {
                secretKey = BigInteger.Parse(data[0].Split(' ').Skip(1).First());
                publicKey = BigInteger.Parse(data[1].Split(' ').Skip(1).First());
                P = BigInteger.Parse(data[2].Split(' ').Skip(1).First());

                var random = new Random();
                K = 3;
            }

            public IEnumerable<BigInteger> MaskMessage(IEnumerable<BigInteger> message)
            {
                var mult = BigInteger.ModPow(K, publicKey, P);
                foreach (var c in message)
                {
                    var m = (c * mult) % P;
                    yield return m;
                }
            }

            public IEnumerable<BigInteger> SignMessage(IEnumerable<BigInteger> maskedMessage)
            {
                foreach (var m in maskedMessage)
                {
                    var t = BigInteger.ModPow(m, secretKey, P);
                    yield return t;
                }
            }

            public IEnumerable<BigInteger> UnmaskMessage(IEnumerable<BigInteger> signedMessage)
            {
                var inverse = K.Inverse(P);
                foreach (var s in signedMessage)
                {
                    var t = (s * inverse) % P;
                    yield return t;
                }
            }        
        }
    }
}