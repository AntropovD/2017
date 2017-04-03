using System.Numerics;
using System.Security.Cryptography;

namespace EllipticCurve.Generator
{
	public class BigIntegerGenerator
	{
		private readonly RNGCryptoServiceProvider provider;

		public BigIntegerGenerator()
		{
			provider = new RNGCryptoServiceProvider();
		}

		public BigInteger GetRandom(int n = 65536)
		{
			var bytes = new byte[n/8];
			provider.GetBytes(bytes);
			return new BigInteger(bytes);
		}
	}
}