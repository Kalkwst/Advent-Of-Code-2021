using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Entities
{
	public class SolutionEntity
	{
		public string Answer { get; set; }
		public TimeSpan ExecutionTime { get; set; }

		public static SolutionEntity Empty => new();
	}
}
