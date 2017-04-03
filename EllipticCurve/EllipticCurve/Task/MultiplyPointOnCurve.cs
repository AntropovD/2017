using System;
using System.IO;
using System.Linq;
using System.Numerics;
using EllipticCurve.Model;

namespace EllipticCurve.Task
{
	public class MultiplyPointOnCurve : ITask
	{
		public string[] Execute(string[] inputData, TextWriter writer, Model.EllipticCurve curve)
		{
			var s = int.Parse(inputData[0]);
			if (inputData.Length < s)
			{
				Console.WriteLine("Error in input data");
				return new string[] {};
			}
			if (s <= 0)
			{
				Console.WriteLine("S in not Natural Number");
				return new string[] {};
			}


			for (var i = 0; i < s; i++)
			{
				var tokens = inputData[i + 1].Split(' ');
				if (tokens.Length != 3)
				{
					writer.WriteLine("Wrong input data in Multiply Task");
				}
				var point = new EllipticCurvePoint(curve);
				point.Read(tokens[0], tokens[1]);

				var d = BigInteger.Parse(tokens[2]);

				var result = point.Multiply(d);
				writer.WriteLine(result);
			}
			return inputData.Skip(s + 1).ToArray();
		}
	}
}