using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Advent_Of_Code
{
	public class Day02 : BaseDay
	{
		public override ValueTask<string> Solve1()
		{
			var data = Utils.ToStringArray(RawData);

			int xPosition = 0;
			int yPosition = 0;

			foreach(string instruction in data)
			{
				string[] commands = instruction.Split(" ");
				switch(commands[0])
				{
					case "forward":
						xPosition += int.Parse(commands[1]);
						break;
					case "up":
						yPosition -= int.Parse(commands[1]);
						break;
					case "down":
						yPosition += int.Parse(commands[1]);
						break;
				}
			}

			return new ValueTask<string>((xPosition * yPosition).ToString());
			
		}

		public override ValueTask<string> Solve2()
		{
			var data = Utils.ToStringArray(RawData);

			int xPosition = 0;
			int yPosition = 0;

			int aim = 0;

			foreach (string instruction in data)
			{
				string[] commands = instruction.Split(" ");
				switch (commands[0])
				{
					case "forward":
						xPosition += int.Parse(commands[1]);
						yPosition += int.Parse(commands[1]) * aim;
						break;
					case "up":
						aim -= int.Parse(commands[1]);
						break;
					case "down":
						aim += int.Parse(commands[1]);
						break;
				}
			}

			return new ValueTask<string>((xPosition * yPosition).ToString());
		}
	}
}
