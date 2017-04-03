using System;

namespace EllipticCurve
{
	public class ArgumentsParser
	{
		private readonly string[] args;
//		private const string inputFilename = "curve.txt";
//		private const string outputFilename = "output.txt";

		public ArgumentsParser(string[] args)
		{
			this.args = args;
			if (args.Length == 2)
			{
				InputFile = args[0];
				OutputFile = args[1];
			}
			InputFile = "curve_example3.txt";
			OutputFile = "output3.txt";
		}

		public string InputFile { get; }
		public string OutputFile { get; }

		public bool IsValid()
		{
			if (args.Length != 2)
			{
				Console.WriteLine("Application for adding and multiplication on elliptic curve over prime P");
				Console.WriteLine("Program reads curve in format y^2 = x^3 + Ax +  (mod P) ");
				Console.WriteLine("Then reads number t and t pair point on curve and adds one point to another ");
				Console.WriteLine("Then reads number s and t elements: point and integer n; and multiplies n*P");
				Console.WriteLine("Usage: EllipticCurve.exe inputFile outputFile");
//				return false;
			}
			return true;
		}
	}
}