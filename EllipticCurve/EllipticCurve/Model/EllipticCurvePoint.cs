using System;
using System.Globalization;
using System.Numerics;
using EllipticCurve.Extensions;

namespace EllipticCurve.Model
{
	public class EllipticCurvePoint
	{
		public EllipticCurve curve;
		public Point point;

		public EllipticCurvePoint()
		{
		}

		public EllipticCurvePoint(EllipticCurve curve)
		{
			this.curve = curve;
			point = new Point();
		}

		public EllipticCurvePoint(EllipticCurve curve, Point point)
		{
			this.curve = curve;
			this.point = point;
		}

		public EllipticCurvePoint(EllipticCurvePoint point)
		{
			curve = point.curve;
			this.point = point.point;
		}


		public BigInteger X
		{
			get { return point.X; }
			set { point.X = value; }
		}

		public BigInteger Y
		{
			get { return point.Y; }
			set { point.Y = value; }
		}

		public BigInteger P => curve.P;

		public void Read(string x, string y)
		{
			point = new Point();

			if (!BigIntegerExtensions.MyParse(x, out point.X))
			{
				Console.WriteLine("Read Error");
				return;
			}

			if (!BigIntegerExtensions.MyParse(y, out point.Y))
			{
				Console.WriteLine("Curve read error B parameter");
				return;
			}
			if (!Check())
			{
				Console.WriteLine("Warning: point doesn't belong to curve");
			}
		}

		private bool Check()
		{
			return (Y*Y - X*X*X - curve.A*X - curve.B)%P == 0;
		}

		public static EllipticCurvePoint operator +(EllipticCurvePoint point1, EllipticCurvePoint point2)
		{
			var result = new EllipticCurvePoint(point1.curve);
			var P = point1.P;

			var dy = point2.Y - point1.Y;
			var dx = point2.X - point1.X;

			if (dx == 0 && dy==0)
			{
				return Double(point1);
			}

			dx += dx < 0 ? P : 0;
			dy += dy < 0 ? P : 0;

			var lambda = dy*dx.ModInverse(P)%P; // Euler's theorem
			lambda += lambda < 0 ? P : 0;

			result.X = (lambda*lambda - point1.X - point2.X)%P;
			result.Y = (lambda*(point1.X - result.X) - point1.Y)%P;

			result.X += result.X < 0 ? P : 0;
			result.Y += result.Y < 0 ? P : 0;

			return result;
		}

		public static EllipticCurvePoint Double(EllipticCurvePoint point)
		{
			var result = new EllipticCurvePoint(point.curve);
			var P = point.P;
			var A = point.curve.A;
			var lambda = (3*point.X*point.X + A)*((2*point.Y).ModInverse(P))%P;

			result.X = (lambda*lambda - 2*point.X)%P;
			result.Y = (lambda*(point.X - result.X) - point.Y)%P;

			result.X += result.X < 0 ? P : 0;
			result.Y += result.Y < 0 ? P : 0;

			return result;
		}

		public static EllipticCurvePoint operator *(EllipticCurvePoint point, int d)
		{
			if (d < 0)
			{
				Console.WriteLine("Error: In multiply on N, N is less 0");
				return point;
			}

			var binaryString = Convert.ToString(d, 2);

			var result = new EllipticCurvePoint(point);

			for (var i = 1; i < binaryString.Length; i++)
			{
				result = Double(result);
				if (binaryString[i] == '1')
					result = result + point;
			}
			return result;
		}

		public override string ToString()
		{
			if (point == null)
				return string.Empty;
			return $"0x{point?.X.ToString("X")} 0x{point?.Y.ToString("X")}";
		}

		public EllipticCurvePoint Multiply(BigInteger N)
		{
			var curvePoint = this;
			if (N < 0)
			{
				Console.WriteLine("Error: In multiply on N, N is less 0");
				return curvePoint;
			}

			var binaryString = N.ToBinaryString();

			var result = new EllipticCurvePoint(curvePoint);

			for (var i = 1; i < binaryString.Length; i++)
			{
				result = Double(result);
				if (binaryString[i] == '1')
					result = result + curvePoint;
			}
			return result;
		}
	}
}