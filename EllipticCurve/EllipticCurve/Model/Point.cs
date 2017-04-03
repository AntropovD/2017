using System.Numerics;

namespace EllipticCurve.Model
{
	public class Point
	{
		public BigInteger X;
		public BigInteger Y;

		public Point()
		{
		}

		public Point(BigInteger x, BigInteger y)
		{
			X = x;
			Y = y;
		}

		public Point(BigInteger x, BigInteger y, BigInteger p)
		{
			if (x < 0) x += p;
			if (y < 0) y += p;
			X = x;
			Y = y;
		}
	}
}