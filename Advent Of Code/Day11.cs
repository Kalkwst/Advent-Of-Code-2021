using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Advent_Of_Code
{
	class Day11 : BaseDay
	{
		private List<Dumbo> Dumbos;

		public override ValueTask<string> Solve1()
		{

			var data = Utils.ToStringArray(RawData);

			BuildDumbos(data);

			Enumerable.Range(0, 100)
				.ToList()
				.ForEach(step => Dumbos.ForEach(dumbo => dumbo.IncreaseEnergy(step)));

			var result = Dumbos.Sum(dumbo => dumbo.HasFlashedThatManyTimes);

			return new ValueTask<string>(result.ToString());
		}

		public override ValueTask<string> Solve2()
		{
			var data = Utils.ToStringArray(RawData);

			BuildDumbos(data);

			Enumerable.Range(0, 100)
				.ToList()
				.ForEach(step => Dumbos.ForEach(dumbo => dumbo.IncreaseEnergy(step)));

			var step = 100;
			while (!Dumbos.All(d => d.HasFlashedForStep(step)))
			{
				++step;
				Dumbos.ForEach(d => d.IncreaseEnergy(step));
			}

			return new ValueTask<string>(step.ToString());
		}

		private void BuildDumbos(string[] data)
		{
			List<string> Entry = data.ToList();
			Dumbos = new List<Dumbo>();
			for (var y = 0; y < Entry.Count; y++)
			{
				var row = Entry[y].Replace("\r", "").ToCharArray()
					.Select(c => Convert.ToInt32(c.ToString()))
					.ToArray();

				for (var x = 0; x < row.Length; x++)
				{
					var newDumbo = new Dumbo
					{
						Point = new Point(x, y),
						Energy = row[x]
					};
					Dumbos.Add(newDumbo);
				}
			}

			Dumbos.ForEach(d => d.FindMyNeighbours(Dumbos));
		}



		internal class Dumbo
		{
			private static readonly List<(int, int)> _PotentialNeighboursCoordinates = new()
			{
				(-1, -1),
				(0, -1),
				(1, -1),
				(-1, 0),
				(1, 0),
				(-1, 1),
				(0, 1),
				(1, 1)
			};

			private readonly Stack<(int Step, bool HasFlashed)> _flashedByStep = new Stack<(int, bool)>();

			private int _energy;

			public int Energy
			{
				get => _energy;
				set
				{
					_energy = value;
					if (_energy < 10) return;

					var currentStep = _flashedByStep.Pop();
					if (!currentStep.HasFlashed)
					{
						_energy = 0;
						currentStep.HasFlashed = true;
					}
					_flashedByStep.Push(currentStep);

					Neighbours.ForEach(n => n.IncreaseEnergy(currentStep.Step));
				}
			}

			public Point Point { get; set; }

			public List<Dumbo> Neighbours { get; } = new List<Dumbo>();

			public int HasFlashedThatManyTimes => _flashedByStep.Count(f => f.HasFlashed);

			public void FindMyNeighbours(List<Dumbo> dumbos)
			{
				void GetMyNeighbourAt((int X, int Y) delta)
				{
					var neighbour = dumbos
						.FirstOrDefault(d => d.Point.X == Point.X + delta.X
											 && d.Point.Y == Point.Y + delta.Y);
					if (neighbour != null) Neighbours.Add(neighbour);
				}
				_PotentialNeighboursCoordinates.ForEach(GetMyNeighbourAt);
			}

			public void IncreaseEnergy(int step)
			{
				if (_flashedByStep.Count != 0
				 && _flashedByStep.Peek().Step == step
				 && _flashedByStep.Peek().HasFlashed)
					return;

				if (_flashedByStep.Count == 0
				 || _flashedByStep.Peek().Step < step)
					_flashedByStep.Push((step, false));

				++Energy;
			}

			public bool HasFlashedForStep(int step) => _flashedByStep.Count != 0
													&& _flashedByStep.Peek().HasFlashed;
		}
	}
}