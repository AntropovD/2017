	using System.IO;

namespace EllipticCurve.Task
{
	public interface ITask
	{
		string[] Execute(string[] inputData, TextWriter writer, Model.EllipticCurve curve);
	}
}