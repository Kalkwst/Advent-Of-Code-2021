using System.IO;
using System.Threading.Tasks;
using Utilities.Bots;
using Utilities.Entities;

namespace Utilities
{
	public abstract class BaseDay
	{
		protected virtual string ClassPrefix { get; } = "Day";
		protected virtual string InputFileDirPath { get; } = $"../../../Data/";

		protected virtual int Year { get; set; } = ConfigEntity.Get("config.json").Year;

		public string RawData { get; set; }

		protected BaseDay()
		{ 
			RawData = DataBot.LoadDebug((int)GetIndex(), Year, InputFilePath);
		}

		public virtual uint GetIndex()
		{
			var typeName = GetType().Name;

			return uint.TryParse(typeName[(typeName.IndexOf(ClassPrefix) + ClassPrefix.Length)..].TrimStart('_'), out var index)
				? index
				: default;
		}

		public virtual string InputFilePath
		{
			get
			{
				var index = GetIndex().ToString("D2");

				return Path.Combine(InputFileDirPath, $"Year{Year}/Day{index}/input/");
			}
		}

		public abstract ValueTask<string> Solve1();
		public abstract ValueTask<string> Solve2();
	}
}
