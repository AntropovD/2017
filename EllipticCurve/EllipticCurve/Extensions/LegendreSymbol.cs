using System.Numerics;

namespace EllipticCurve.Extensions
{
	public static class LegendreSymbol
	{
		public static BigInteger GetLegandre(BigInteger a, BigInteger p)
		{
			if (a == 1)
			{
				return 1;
			}
			if (a%2 == 0)
			{
				if ((p*p - 1)/8%2 == 0)
					return GetLegandre(a/2, p);
				return -GetLegandre(a/2, p);
			}
			if ((a%2 != 0) && (a != 1))
			{
				if ((a - 1)*(p - 1)/4%2 == 0)
					return GetLegandre(p%a, a);
				return -GetLegandre(p%a, a);
			}
			return 0;
		}
	}
}