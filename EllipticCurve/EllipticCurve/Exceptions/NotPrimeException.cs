using System;

namespace EllipticCurve.Exceptions
{
	public class NotPrimeException : Exception
	{
		public NotPrimeException() : base("P characteristic is not prime")
		{
		}
	}
}
