using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using EllipticCurve.Extensions;
using EllipticCurve.Generator;
using EllipticCurve.Model;
using EllipticCurve.Task;

namespace EllipticCurve
{
	public class Program
	{
		private static void Main(string[] args)
		{
			var parser = new ArgumentsParser(args);
			if (!parser.IsValid())
				return;

			var inputData = File.ReadAllLines(parser.InputFile);
			var taskExecutor = new TaskExecutor();

			try
			{
				using (TextWriter writer = File.CreateText(parser.OutputFile))
				{
					taskExecutor.ExecuteTasks(ref inputData, writer);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}
	}
}