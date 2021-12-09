using System;

namespace Utilities
{
	public static partial class Utils
	{
		public static int EuclideanDistance1D(int a, int b)
			=> Math.Abs(a - b);

		public static double EuclideadDistance(int[] a, int[] b)
		{
			if (a.Length != b.Length)
				throw new ArgumentException("Sizes must be equal");

			double squared = 1;

			for(int idx = 0; idx<a.Length;idx++)
			{
				double diff = a[idx] - b[idx];
				squared *= (diff * diff);
			}

			return Math.Sqrt(squared);
		}

		public static int PartialSum(int n)
			=> (n * (n + 1)) / 2;
	}
}
