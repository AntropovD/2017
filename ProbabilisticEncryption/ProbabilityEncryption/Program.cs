using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace ConsoleApplication1
{
	internal class Program
	{
		public static void Main()
		{
			var generator = new KeyGenerator();
			var key = generator.GenerateKey();

			Console.WriteLine("Key is generated");
			Console.WriteLine(key);

			Console.WriteLine("Enter message:");
			var message = Console.ReadLine();
			if (string.IsNullOrEmpty(message)) throw new ArgumentNullException();

			Console.WriteLine("Encryption:");
			var encryptedText = ProbabilisticEncryption.Encrypt(message, key.PublicKey);
			Console.WriteLine($"EncryptedText = {encryptedText}");

			Console.WriteLine("Decryption:");
			var decryptedText = ProbabilisticEncryption.Decrypt(encryptedText, key, message.Length);
			Console.WriteLine($"DecryptedText = {decryptedText}");
		}
	}

	public class ProbabilisticEncryption
	{
		public static string Encrypt(string text, PublicKey key)
		{
			var generator = new BbsGenerator(key.N);

			var result = new StringBuilder();
			foreach (var c in text)
			{
				var b = generator.GetNext();
				var t = c;
				result.Append((char)(t ^ b));
			}
			generator.GetNext();
			result.Append($"#{generator.X}");
			return result.ToString();
		}

		public static string Decrypt(string encryptedText, Key key, int length)
		{
			var seed = GetLastSeed(encryptedText);
			var x0 = RestoreX0(key, length, seed);
			Console.WriteLine($"Restored x0={x0}");

			var generator = new BbsGenerator(key.PublicKey.N, x0);
			var result = new StringBuilder();
			for (var i = 0; i < length; i++)
			{
				var b = generator.GetNext();
				var t = encryptedText[i];
				result.Append((char)(t ^ b));
			}
			return result.ToString();
		}

		private static long GetLastSeed(string text)
		{
			var pos = text.LastIndexOf('#');
			return Convert.ToInt64(text.Substring(pos + 1));
		}

		private static long RestoreX0(Key key, int t, long xt)
		{
			long a, b;
			var p = key.SecretKey.P;
			var q = key.SecretKey.Q;
			GcdExtended(p, q, out a, out b);

			var u = BigInteger.ModPow(new BigInteger((int)((p + 1) / 4)), new BigInteger(t + 1), new BigInteger(p - 1));
			var v = BigInteger.ModPow(new BigInteger((int)((q + 1) / 4)), new BigInteger(t + 1), new BigInteger(q - 1));
			var w = BigInteger.ModPow(new BigInteger(xt), u, new BigInteger(p));
			var z = BigInteger.ModPow(new BigInteger(xt), v, new BigInteger(q));

			var res = (z * a * p + w * q * b) % key.PublicKey.N;
			if (res < 0) res += key.PublicKey.N;
			return (long)res;
		}

		private static long GcdExtended(long a, long b, out long x, out long y)
		{
			if (a == 0)
			{
				x = 0;
				y = 1;
				return b;
			}

			long x1, y1;
			var res = GcdExtended(b % a, a, out x1, out y1);

			x = y1 - b / a * x1;
			y = x1;
			return res;
		}
	}

	public class BbsGenerator
	{
		private int i;
		public long N;
		public long X;

		public BbsGenerator(long n, long? value = null)
		{
			N = n;
			if (value.HasValue)
			{
				X = value.Value;
			}
			else
			{
				var rand = new Random();
				X = rand.Next((int)n);
				X = X * X % n;
				Console.WriteLine($"Generated x0 = {X}");
			}
			i = 0;
		}

		public byte GetNext()
		{
			X = X * X % N;
			Console.WriteLine($"Generated x{++i} = {X}");
			return (byte)(X & 1);
		}
	}

	public class Key
	{
		public PublicKey PublicKey { get; set; }
		public SecretKey SecretKey { get; set; }

		public override string ToString()
		{
			return $"PublicKey: {PublicKey} SecretKey: {SecretKey}";
		}
	}

	public class PublicKey
	{
		public long N;

		public override string ToString()
		{
			return $"N = {N}";
		}
	}

	public class SecretKey
	{
		public long P;
		public long Q;

		public override string ToString()
		{
			return $"P={P}, Q={Q}";
		}
	}

	public class KeyGenerator
	{
		public const int N = 1024;
		public List<int> Primes;

		public Key GenerateKey()
		{
			Primes = PrimeNumbersGenerator.GeneratePrimesNaive(N);

			while (true)
			{
				var p = FindMod3();
				var q = FindMod3();
				if (p.HasValue && q.HasValue && p != q)
					return new Key
					{
						PublicKey = new PublicKey
						{
							N = p.Value * q.Value
						},
						SecretKey = new SecretKey
						{
							P = p.Value,
							Q = q.Value
						}
					};
			}
		}

		private int? FindMod3()
		{
			var rand = new Random();
			Primes = Primes.OrderBy(c => rand.Next()).ToList();

			foreach (var prime in Primes)
				if (prime % 4 == 3)
					return prime;
			return null;
		}
	}

	public class PrimeNumbersGenerator
	{
		public static List<int> GeneratePrimesNaive(int n)
		{
			var primes = new List<int> { 2 };
			var nextPrime = 3;
			while (primes.Count < n)
			{
				var sqrt = (int)Math.Sqrt(nextPrime);
				var isPrime = true;
				for (var i = 0; primes[i] <= sqrt; i++)
					if (nextPrime % primes[i] == 0)
					{
						isPrime = false;
						break;
					}
				if (isPrime)
					primes.Add(nextPrime);
				nextPrime += 2;
			}
			return primes;
		}
	}
}