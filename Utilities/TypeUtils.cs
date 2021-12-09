using System.Linq;

namespace Utilities
{
	public static partial class Utils
	{
		public static string[] ToStringArray(string data, string separator = "\n")
			=> data.Split(separator);

		public static int[] ToIntArray(string data, string separator = "\n")
			=> data.Split(separator).Select(x => int.Parse(x)).ToArray();

		public static long[] ToLongArray(string data, string separator = "\n")
			=> data.Split(separator).Select(x => long.Parse(x)).ToArray();

		public static ulong[] ToUnsignedLongArray(string data, string separator = "\n")
			=> data.Split(separator).Select(x => ulong.Parse(x)).ToArray();
	}
}
