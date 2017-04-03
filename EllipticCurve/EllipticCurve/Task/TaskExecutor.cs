using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EllipticCurve.Task
{
	public class TaskExecutor
	{
		private List<Func<string[], TextWriter, Model.EllipticCurve, string[]>> tasks;

		public TaskExecutor()
		{
			tasks = new List<Func<string[], TextWriter, Model.EllipticCurve, string[]>>
			{
				new SumPointsOnCurve().Execute,
				new MultiplyPointOnCurve().Execute
			};
		}

		public void ExecuteTasks(ref string[] inputData, TextWriter writer)
		{
			var curve = new Model.EllipticCurve();
			curve.Read(ref inputData);

			foreach (var task in tasks)
			{
				inputData = task.Invoke(inputData, writer, curve);
			}
		}
	}
}