using System;
using System.Globalization;
using System.Linq;
using System.Numerics;
using EllipticCurve.Exceptions;
using EllipticCurve.Extensions;

namespace EllipticCurve.Model
{
	public class EllipticCurve
	{
		public BigInteger A;
		public BigInteger B;
		public BigInteger P;

		public EllipticCurve()
		{
		}

		public EllipticCurve(EllipticCurve curve)
		{
			A = curve.A;
			B = curve.B;
			P = curve.P;
		}

		public void Read(ref string[] lines)
		{
			P = BigInteger.Parse(lines[0]);
			if (!P.IsProbablePrime())
				throw new NotPrimeException();

			if (!BigIntegerExtensions.MyParse(lines[1], out A))
			{
				Console.WriteLine("Curve read error A parameter");
				return;
			}

			if (!BigIntegerExtensions.MyParse(lines[2], out B))
			{
				Console.WriteLine("Curve read error B parameter");
			}
			lines = lines.Skip(3).ToArray();
		}

		public bool IsNonSingular()
		{
			return 4*BigInteger.Pow(A, 3) + 27*BigInteger.Pow(B, 2) != BigInteger.Zero;
		}

		public bool Check(BigInteger x, BigInteger y)
		{
			var result = (BigInteger.Pow(y, 2) - BigInteger.Pow(x, 3) - A*x - B)%P;
			return result == BigInteger.Zero;
		}
	}
}