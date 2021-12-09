using System;
using System.IO;
using System.Net;
using Utilities.Entities;

namespace Utilities.Bots
{
	public static class DataBot
	{
		private static readonly string Session = ConfigEntity.Get("config.json").Session;

		public static string LoadIInput(int day, int year, string path)
		{
			string url = GetRemoteDataURL(day, year);
			string data = "";

			if (File.Exists(path) && new FileInfo(path).Length > 0)
				return File.ReadAllText(Path.Combine(path, "data.txt"));

			try
			{
				DateTime current = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Utc).AddHours(-5);
				if (current < new DateTime(year, 12, day))
					throw new ArgumentOutOfRangeException("Cannot see into the future!");

				using var client = new WebClient();
				client.Headers.Add(HttpRequestHeader.Cookie, Session);
				data = client.DownloadString(url).Trim();

				FileInfo file = new(path);
				file.Directory.Create();
				File.WriteAllText(Path.Combine(path, "data.txt"), data);
			}
			catch (WebException e)
			{
				var statusCode = ((HttpWebResponse)e.Response).StatusCode;

				switch (statusCode)
				{
					case HttpStatusCode.BadRequest:
						Console.WriteLine($"Day {day}: ERROR CODE 400. Session cookie is not set or malformed.");
						break;
					case HttpStatusCode.NotFound:
						Console.WriteLine($"Day {day}: ERROR CODE 404. Data not found.");
						break;
					default:
						Console.WriteLine(e.ToString());
						break;
				}
			}

			return data;
		}

		public static string LoadDebug(int day, int year, string path)
		{
			if (File.Exists(path) && new FileInfo(path).Length > 0)
				return File.ReadAllText(Path.Combine(path, "debug.txt"));

			return LoadIInput(day, year, path);
		}

		private static string GetRemoteDataURL(int day, int year)
		{
			return $"https://adventofcode.com/{year}/day/{day}/input";
		}
	}
}
