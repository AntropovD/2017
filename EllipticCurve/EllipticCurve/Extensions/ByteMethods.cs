using System;

namespace EllipticCurve.Extensions
{
	public static class ByteMethods
	{
		public static byte[] GetBytes(string str)
		{
			var bytes = new byte[str.Length*sizeof (char)];
			Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
			return bytes;
		}

		public static string GetString(byte[] bytes)
		{
			var chars = new char[bytes.Length/sizeof (char)];
			Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
			return new string(chars);
		}
	}
}