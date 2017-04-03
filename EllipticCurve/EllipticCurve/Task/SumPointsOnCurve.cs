using System;
using System.IO;
using System.Linq;
using EllipticCurve.Extensions;
using EllipticCurve.Model;

namespace EllipticCurve.Task
{
	public class SumPointsOnCurve : ITask
	{
		public string[] Execute(string[] inputData, TextWriter writer, Model.EllipticCurve curve)
		{
			var t = int.Parse(inputData[0]);
			if (inputData.Length < t)
				Console.WriteLine("Error in input data");
			for (int i = 0; i < t; i++)
			{
				var tokens = inputData[i + 1].Split(' ');
				if (tokens.Length != 4)
				{
					writer.WriteLine("Wrong input data in Sum Task");
				}
				var point1 = new EllipticCurvePoint(curve);
				point1.Read(tokens[0], tokens[1]);
				var point2 = new EllipticCurvePoint(curve);
				point2.Read(tokens[2], tokens[3]);

				var result = point1 + point2;
				writer.WriteLine(result);
			}
			return inputData.Skip(t + 1).ToArray();
		}
	}
}