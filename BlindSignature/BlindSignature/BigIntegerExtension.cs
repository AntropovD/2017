using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Globalization;
using System.Security.Cryptography;

namespace BlindSignature
{
    public static class BigIntegerExtensions
	{
		public static BigInteger Inverse(this BigInteger value, BigInteger Module)
		{
			if (value < 0)
				value += Module;
			var a = value;
			var b = Module;
			var x = BigInteger.Zero;
			var d = BigInteger.One;
			
			while (a > 0)
			{
				var q = b / a;
				var y = a;
				a = b % a;
				b = y;
				y = d;
				d = x - q*d;
				x = y;
			}
			x = x % Module;
			if (x < 0)
			{
				x += Module;
			}
			return x;
		}
    }
}
