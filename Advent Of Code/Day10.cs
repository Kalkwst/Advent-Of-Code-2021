using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Advent_Of_Code
{
	class Day10 : BaseDay
	{
		private List<char> IllegalCharacters;
		private List<List<char>> IncompleteSequences = new();
		private List<List<char>> ClosingSequences = new();
		private Stack<char> brackets;

		public override ValueTask<string> Solve1()
		{
			IllegalCharacters = new();

			var data = Utils.ToStringArray(RawData);

			foreach (string line in data)
				GetIllegalCharacters(line);

			long points = CalculatePoints();

			return new ValueTask<string>(points.ToString());
		}

		public override ValueTask<string> Solve2()
		{
			IllegalCharacters = new();
			IncompleteSequences = new();
			ClosingSequences = new();

			var data = Utils.ToStringArray(RawData);

			foreach (string line in data)
				GetIllegalCharacters(line);

			foreach (var sequence in IncompleteSequences)
				GenerateClosingSequence(sequence);

			return new ValueTask<string>(CalculateFinalScore(ClosingSequences).ToString());
		}

		private void GetIllegalCharacters(string line)
		{
			brackets = new();

			char[] characters = line.Replace("\r", "").ToCharArray();

			for(int i = 0; i < characters.Length; i++)
			{
				if (IsOpeningBracket(characters[i]))
				{
					brackets.Push(characters[i]);
					continue;
				}

				if (!IsMatching(characters[i]))
				{
					IllegalCharacters.Add(characters[i]);
					return;
				}
			}

			GetIncompleteCharacters(brackets);
		}

		private void GetIncompleteCharacters(Stack<char> brackets)
		{
			List<char> characterSet = new();
			foreach(char bracket in brackets)
			{
				characterSet.Add(bracket);
			}

			IncompleteSequences.Add(characterSet);
		}

		private void GenerateClosingSequence(List<char> openingSequence)
		{
			List<char> sequence = new();

			foreach(char character in openingSequence)
			{
				if (character.Equals('('))
					sequence.Add(')');
				if (character.Equals('['))
					sequence.Add(']');
				if (character.Equals('{'))
					sequence.Add('}');
				if (character.Equals('<'))
					sequence.Add('>');

			}

			ClosingSequences.Add(sequence);
		}

		private long CalculatePointsSequence(List<char> sequence)
		{
			long points = 0;

			foreach(char character in sequence)
			{
				points *= 5;
				if (character.Equals(')'))
					points += 1;
				if (character.Equals(']'))
					points += 2;
				if (character.Equals('}'))
					points += 3;
				if (character.Equals('>'))
					points += 4;
			}

			return points;
		}

		private long CalculateFinalScore(List<List<char>> sequences)
		{
			long[] scores = new long[sequences.Count];
			for(int i = 0; i < scores.Length; i++)
			{
				scores[i] = CalculatePointsSequence(sequences[i]);
			}

			Array.Sort(scores);

			return scores[sequences.Count / 2];
		}

		private long CalculatePoints()
		{
			long points = 0;

			foreach(char character in IllegalCharacters)
			{
				if (character.Equals(')'))
					points += 3;
				if (character.Equals(']'))
					points += 57;
				if (character.Equals('}'))
					points += 1197;
				if (character.Equals('>'))
					points += 25137;
			}

			return points;
		}

		private bool IsMatching(char v)
		{
			char opening = brackets.Pop();

			if (v.Equals(')') && opening.Equals('('))
				return true;
			if (v.Equals(']') && opening.Equals('['))
				return true;
			if (v.Equals('}') && opening.Equals('{'))
				return true;
			if (v.Equals('>') && opening.Equals('<'))
				return true;

			return false;
		}

		private bool IsOpeningBracket(char v)
			=> v.Equals('(') ||
			   v.Equals('[') ||
			   v.Equals('{') ||
			   v.Equals('<');
	}
}
