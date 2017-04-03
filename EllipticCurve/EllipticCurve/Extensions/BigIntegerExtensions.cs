using System;
using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace EllipticCurve.Extensions
{
	public static class BigIntegerExtensions
	{
		private const int Certainity = 50;

		public static bool IsProbablePrime(this BigInteger source, int certainty = Certainity)
		{
			if (source == 2 || source == 3)
				return true;
			if (source < 2 || source%2 == 0)
				return false;

			var d = source - 1;
			var s = 0;

			while (d%2 == 0)
			{
				d /= 2;
				s += 1;
			}

			var rng = RandomNumberGenerator.Create();
			var bytes = new byte[source.ToByteArray().LongLength];
			BigInteger a;

			for (var i = 0; i < certainty; i++)
			{
				do
				{
					rng.GetBytes(bytes);
					a = new BigInteger(bytes);
				} while (a < 2 || a >= source - 2);

				var x = BigInteger.ModPow(a, d, source);
				if (x == 1 || x == source - 1)
					continue;

				for (var r = 1; r < s; r++)
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

		public static BigInteger Sqrt(this BigInteger n)
		{
			if (n == 0) return 0;
			if (n > 0)
			{
				var bitLength = Convert.ToInt32(Math.Ceiling(BigInteger.Log(n, 2)));
				var root = BigInteger.One << (bitLength/2);

				while (!IsSqrt(n, root))
				{
					root += n/root;
					root /= 2;
				}

				return root;
			}
			return BigInteger.Zero;
		}

		public static BigInteger ModInverse(this BigInteger source, BigInteger P)
		{
			return BigInteger.ModPow(source, P - 2, P);
		}

		public static bool IsSqrt(BigInteger n, BigInteger root)
		{
			var lowerBound = root*root;
			var upperBound = (root + 1)*(root + 1);

			return n >= lowerBound && n < upperBound;
		}

		public static string ToBinaryString(this BigInteger bigint)
		{
			var bytes = bigint.ToByteArray();
			var idx = bytes.Length - 1;

			// Create a StringBuilder having appropriate capacity.
			var base2 = new StringBuilder(bytes.Length * 8);

			// Convert first byte to binary.
			var binary = Convert.ToString(bytes[idx], 2);

			// Ensure leading zero exists if value is positive.
			if (binary[0] != '0' && bigint.Sign == 1)
			{
				base2.Append('0');
			}

			// Append binary string to StringBuilder.
			base2.Append(binary);

			// Convert remaining bytes adding leading zeros.
			for (idx--; idx >= 0; idx--)
			{
				base2.Append(Convert.ToString(bytes[idx], 2).PadLeft(8, '0'));
			}

			return base2.ToString();
		}

		public static bool MyParse(string data, out BigInteger value)
		{
			if (data.StartsWith("0x", StringComparison.CurrentCulture))
			{
				data = data.Replace("0x", String.Empty);
				return BigInteger.TryParse(data, NumberStyles.HexNumber, null, out value);
			}
			return  BigInteger.TryParse(data, out value);
		}
	}
}