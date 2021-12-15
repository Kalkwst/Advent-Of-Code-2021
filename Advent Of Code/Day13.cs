using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Advent_Of_Code
{
	class Day13 : BaseDay
	{
		private static List<(int, int)> points;
		public override ValueTask<string> Solve1()
		{

			RawData = @"6,10
0,14
9,10
0,3
10,4
4,11
6,0
6,12
4,1
0,13
10,12
3,4
3,0
8,4
1,10
2,14
8,10
9,0

fold along y=7
fold along x=5";
			//var data = Utils.ToStringArray(RawData.Replace("\r", ""));

			points = new();

			var dataPoints = RawData.Replace("\r", "").Split("\n\n")[0].Split("\n");
			var folds = RawData.Replace("\r", "").Split("\n\n")[1].Replace("fold along ","").Split("\n");

			foreach(var dataPoint in dataPoints)
			{
				var coordinates = dataPoint.Split(',');
				var x = Convert.ToInt32(coordinates[0]);
				var y = Convert.ToInt32(coordinates[1]);

				(int xPosition, int yPosition) point = (x,y);

				points.Add(point);
			}

			var xFold = Convert.ToInt32(folds[0].Split("=")[1]);
			var yFold = Convert.ToInt32(folds[1].Split("=")[1]);


			points.Select(point => point.Item1 = RecalculateXPosition(point, xFold));
			points.Select(point => point.Item2 = RecalculateYPosition(point, yFold));

			long result = points.Distinct().Count();
			return new ValueTask<string>(result.ToString());
		}

		public override ValueTask<string> Solve2()
		{
			throw new NotImplementedException();
		}

		private int RecalculateXPosition((int xPosition, int yPosition) point, int foldingPoint)
			=> Math.Abs(point.xPosition - foldingPoint) + point.xPosition;

		private int RecalculateYPosition((int xPosition, int yPosition) point, int foldingPoint)
			=> point.yPosition - Math.Abs(point.yPosition - foldingPoint);
	}
}
