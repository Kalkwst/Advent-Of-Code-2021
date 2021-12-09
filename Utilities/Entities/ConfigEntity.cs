using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utilities.Entities
{
	public class ConfigEntity
	{
		string session;
		int year;
		int[] days;

		public string Session
		{
			get => session;
			set
			{
				if (Regex.IsMatch(value, "^session=[a-z0-9]+$"))
					session = value;
			}
		}

		public int Year
		{
			get => year;
			set
			{
				if (value >= 2015 && value <= DateTime.Now.Year)
					year = value;
			}
		}

		[JsonConverter(typeof(DaysConverter))]
		public int[] Days
		{
			get => days;
			set
			{
				bool allDaysCovered = false;
				days = value.Where(v =>
				{
					if (v == 0)
						allDaysCovered = true;

					return v > 0 && v < 26;
				}).ToArray();

				if (allDaysCovered)
					days = new int[] { 0 };
				else
					Array.Sort(days);
			}
		}

		private void SetDefaults()
		{
			DateTime currentEST = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Utc).AddHours(-5);
			if (Session == default)
				Session = "";

			if (Year == default)
				Year = currentEST.Year;

			if (Days == default)
				Days = (currentEST.Month == 12 && currentEST.Day <= 25)
					? new int[] { currentEST.Day }
					: new int[] { 0 };
		}

		public static ConfigEntity Get(string path)
		{
			var options = new JsonSerializerOptions()
			{
				IgnoreNullValues = true,
				PropertyNameCaseInsensitive = true,
				WriteIndented = true
			};

			ConfigEntity config;
			if(File.Exists(path))
			{
				//Console.WriteLine($"Found config.json under {Path.GetFullPath(path)}");
				config = JsonSerializer.Deserialize<ConfigEntity>(File.ReadAllText(path), options);
				config.SetDefaults();
			}
			else
			{
				Console.WriteLine($"Created config.json under {Path.GetFullPath(path)}");
				config = new ConfigEntity();
				config.SetDefaults();
				File.WriteAllText(path, JsonSerializer.Serialize<ConfigEntity>(config, options));
			}

			return config;
		}
	}

	internal class DaysConverter : JsonConverter<int[]>
	{
		public override int[] Read(ref Utf8JsonReader reader, Type typeToConverter, JsonSerializerOptions opts)
		{
			if (reader.TokenType == JsonTokenType.Number)
				return new int[] { reader.GetInt16() };

			var tokens = reader.TokenType == JsonTokenType.String
				? new string[] { reader.GetString() }
				: JsonSerializer.Deserialize<object[]>(ref reader)
					.Select<object, string>(o => o.ToString());

			return tokens.SelectMany<string, int>(ParseString).ToArray();
		}

		private IEnumerable<int> ParseString(string arg)
		{
			return arg.Split(",").SelectMany<string, int>(str =>
			{
				if (str.Contains(".."))
				{
					var split = str.Split("..");
					var start = int.Parse(split[0]);
					var stop = int.Parse(split[1]);

					return Enumerable.Range(start, stop - start + 1);
				}
				else if (int.TryParse(str, out int day))
					return new int[] { day };

				return Array.Empty<int>();
			});
		}

		public override void Write(Utf8JsonWriter writer, int[] value, JsonSerializerOptions options)
		{
			writer.WriteStartArray();
			foreach (int v in value)
				writer.WriteNumberValue(v);

			writer.WriteEndArray();
		}
	}
}
