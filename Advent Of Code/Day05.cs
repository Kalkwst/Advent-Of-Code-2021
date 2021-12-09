using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Advent_Of_Code
{
	public class Day05 : BaseDay
	{
		public override ValueTask<string> Solve1()
		{
			return new ValueTask<string>(ProcessMap(false, 1, Utils.ToStringArray(RawData)).ToString());
		}

		public override ValueTask<string> Solve2()
		{
			return new ValueTask<string>(ProcessMap(true, 1, Utils.ToStringArray(RawData)).ToString());
		}

		private long ProcessMap(bool useDiagonals, int intersections, string[] data)
		{
			var coordinates = data.Select(x => x.Split(" -> ")).Select(coords => (new Point(coords[0]), (new Point(coords[1]))));
			var map = new VentMap(useDiagonals);

			foreach (var vent in coordinates)
				map.AddLine(vent.Item1, vent.Item2);

			return map.GetPointsWithMoreThanNIntersections(intersections);
		}
	}

	public class VentMap
	{
		private readonly bool useDiagonals;

		private readonly Dictionary<string, int> vents;

		public VentMap(bool useDiagonals)
		{
			this.useDiagonals = useDiagonals;
			vents = new Dictionary<string, int>();
		}

		public void AddLine(Point from, Point to)
		{
			if(ValidateCoordinates(from, to))
			{
				IEnumerable<Point> points = from.GetPointsBetween(to);

				foreach(var point in points)
				{
					if (vents.ContainsKey(point.X + "," + point.Y))
						vents[point.X + "," + point.Y] = vents[point.X + "," + point.Y] + 1;
					else
						vents[point.X + "," + point.Y] = 1;
				}
			}
		}

		public int GetPointsWithMoreThanNIntersections(int intersections)
			=> vents.Values.Count(i => i > intersections);

		private bool ValidateCoordinates(Point from, Point to)
			=> from.IsHorizonal(to) ||
			   from.IsVertial(to) ||
			   from.IsDiagonal(to) &&
			   useDiagonals;
	}

	public class Point
	{
		public int X { get; }
		public int Y { get; }

		public Point(int x, int y)
		{
			X = x;
			Y = y;
		}

		public Point(string str)
		{
			var splitCoords = str.Split(",");
			X = int.Parse(splitCoords[0]);
			Y = int.Parse(splitCoords[1]);
		}

		public IEnumerable<Point> GetPointsBetween(Point other)
		{
			if (IsHorizonal(other))
				return Utils.Sequence(X, other.X)
							.Select(x => new Point(x, Y));

			if (IsVertial(other))
				return Utils.Sequence(Y, other.Y)
							.Select(y => new Point(X, y));

			if (IsDiagonal(other))
				return Utils.Sequence(X, other.X)
							.Zip(Utils.Sequence(Y, other.Y))
							.Select(coords => new Point(coords.First, coords.Second));

			return Enumerable.Empty<Point>();
		}

		public bool IsHorizonal(Point other)
			=> Y == other.Y;

		public bool IsVertial(Point other)
			=> X == other.X;

		public bool IsDiagonal(Point other)
			=> Math.Abs(X - other.X) == Math.Abs(Y - other.Y);
	}
}
