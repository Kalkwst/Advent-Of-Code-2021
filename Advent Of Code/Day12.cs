using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Advent_Of_Code
{
	class Day12 : BaseDay
	{
		public override ValueTask<string> Solve1()
		{
			var data = Utils.ToStringArray(RawData).ToList();
			var graph = new CaveGraph();

			data.ForEach(row =>
			{
				var edge = row.Split('-');
				graph.AddEdge(edge[0], edge[1].Replace("\r", ""));
			});

			return new ValueTask<string>(graph.CalculateAllPaths("start", "end").ToString());
			
		}

		public override ValueTask<string> Solve2()
		{
			var data = Utils.ToStringArray(RawData).ToList();
			var graph = new CaveGraph();

			data.ForEach(row =>
			{
				var edge = row.Split('-');
				graph.AddEdge(edge[0], edge[1].Replace("\r", ""));
			});

			return new ValueTask<string>(graph.CalculateAllPaths("start", "end", 2).ToString());
		}

		internal class CaveGraph
		{
			private readonly Dictionary<string, List<string>> Vertices = new();
			private readonly Stack<string> CurrentPath = new();
			private readonly List<List<string>> AllPaths = new();

			public void AddEdge(string v, string w)
			{
				if (!Vertices.ContainsKey(v))
					Vertices.Add(v, new List<string>());

				if (!Vertices.ContainsKey(w))
					Vertices.Add(w, new List<string>());

				Vertices[v].Add(w);
				Vertices[w].Add(v);
			}

			public int CalculateAllPaths(string start, string end, int round = 1)
			{
				CurrentPath.Clear();
				AllPaths.Clear();

				TraverseDFS(start, end, round == 1 ? VisitOnce : VisitTwice);
				return AllPaths.Count();
			}

			public void TraverseDFS(string v, string w, Func<string, bool> visitLimiter)
			{
				if (v.Equals("start") && CurrentPath.Contains("start"))
					return;

				CurrentPath.Push(v);

				if (v.Equals(w))
				{
					AllPaths.Add(CurrentPath.Reverse().ToList());
					CurrentPath.Pop();
					return;
				}

				if (char.IsLower(v[0]) && visitLimiter(v))
				{
					CurrentPath.Pop();
					return;
				}

				foreach (var adjacent in Vertices[v])
					TraverseDFS(adjacent, w, visitLimiter);

				CurrentPath.Pop();
			}

			private bool VisitOnce(string v)
				=> CurrentPath.Count(p => p == v) > 1;

			private bool VisitTwice(string v)
				=> (CurrentPath
						.Where(c => char.IsLower(c[0]) && c != v)
						.GroupBy(c => c)
						.Any(g => g.Count() > 1)
					&& CurrentPath.Count(c => c == v) == 2
					|| CurrentPath.Count(c => c == v) > 2);
		}
	}
}
