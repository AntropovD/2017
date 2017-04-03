using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Numerics;

namespace KeyEscrow
{
    class Program
    {
        const string filename = "config.txt";

        static void Main(string[] args)
        {
            var data = File.ReadAllLines(filename);
            if (data.Length != 3)
            {
                Console.WriteLine("Wrong config file");
                return;
            }

            var task = new KeyEscrowExample();
            task.Run(data);
        }
    }

    class KeyEscrowExample
    {
        BigInteger secret;
        BigInteger P;
        BigInteger G;

        public void Run(string[] data)
        {
            Init(data);          
            
            var s = DivideSecret().ToArray();
            BigInteger[] t = new BigInteger[5];
            for (int i = 0; i < 5; i++)
            {
                t[i] = BigInteger.ModPow(G, s[i], P);
                Console.WriteLine(string.Format("Alice sends C{0} value t{1} = {2}", i, i, t[i].ToString()));
            }
            var T = BigInteger.ModPow(G, secret, P);
            Console.WriteLine(string.Format("Alice sends Bob value T = {0}", T.ToString()));

            Console.WriteLine("Bob checks if T == t1*..t5");
            BigInteger result = BigInteger.One;;
            for (int i = 0; i < 5; i++)
                result = (result * t[i]) % P;
            Console.WriteLine("{0} = {1}", T, result);
            if (T == result)
                Console.WriteLine("Alice doesn't lie");
            else 
                Console.WriteLine("Alice lies");
        }

        void Init(string[] data)
        {
            secret = BigInteger.Parse(data[0].Split(' ').Skip(1).First());
            P = BigInteger.Parse(data[1].Split(' ').Skip(1).First());
            G = BigInteger.Parse(data[2].Split(' ').Skip(1).First());
        }

        IEnumerable<BigInteger> DivideSecret()
        {
            var div5 = secret / 5;
            var sum = BigInteger.Zero;
            for (int i = 0; i < 4; i++)
            {
                var t = RandomIntegerBelow(div5);
                sum += t;
                yield return t;
            }
            yield return secret - sum;
        }

        void CheckSum(BigInteger[] parts)
        {
            BigInteger res = BigInteger.Zero;
            for (int i = 0; i < parts.Length; i++)
                res += parts[i];
            Console.WriteLine(res == secret);
        }

        static BigInteger RandomIntegerBelow(BigInteger N)
        {
            byte[] bytes = N.ToByteArray();
            BigInteger R;
            var random = new Random();
            do
            {
                random.NextBytes(bytes);
                bytes[bytes.Length - 1] &= (byte)0x7F; //force sign bit to positive
                R = new BigInteger(bytes);
            } while (R >= N);

            return R;
        }
    }

    static class StringExtensions
    {
        public static IEnumerable<string> Split(this string s, int chunkSize)
        {
            int chunkCount = s.Length / chunkSize;
            for (int i = 0; i < chunkCount; i++)
                yield return s.Substring(i * chunkSize, chunkSize);

            if (chunkSize * chunkCount < s.Length)
                yield return s.Substring(chunkSize * chunkCount);
        }
    }
}
