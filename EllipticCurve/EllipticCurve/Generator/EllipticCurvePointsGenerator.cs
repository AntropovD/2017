using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using EllipticCurve.Extensions;
using EllipticCurve.Model;

namespace EllipticCurve.Generator
{
	public class EllipticCurvePointsGenerator
	{
		private Model.EllipticCurve curve;

		public EllipticCurvePointsGenerator(Model.EllipticCurve curve)
		{
			this.curve = curve;
		}

		public void Run()
		{
			var lines = File.ReadAllLines("curve.txt");
			curve = new Model.EllipticCurve();
			curve.Read(ref lines);
			
			IEnumerable<EllipticCurvePoint> result = new List<EllipticCurvePoint>();
			try
			{
				result = GeneratePoints();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}

			File.WriteAllLines("points.txt", result.Select(x => x.ToString()));
		}

		//Generates point on not large P
		//Just for test
		public IEnumerable<EllipticCurvePoint> GeneratePoints()
		{
			for (var i = 0; i < 1000000; i++)
			{
				var x = new BigInteger(i);
				var value = (BigInteger.Pow(x, 3) + curve.A*x + curve.B)%curve.P;
				if (value < 0) value += curve.P;

				if (LegendreSymbol.GetLegandre(value, curve.P) == 1)
				{
					for (var j = 0; j < 100000; j++)
					{
						var temp = value + j*curve.P;
						var y = temp.Sqrt();
						if (BigInteger.Pow(y, 2)%curve.P == value)
						{
							yield return new EllipticCurvePoint
							{
								curve = curve,
								point = new Point(x, y, curve.P)
							};
						}
						break;
					}
				}
			}
		}
	}
}