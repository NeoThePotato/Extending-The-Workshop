﻿static class Utility
{
	public static void Swap<T>(ref T obj1, ref T obj2)
	{
		var temp = obj1;
		obj1 = obj2;
		obj2 = temp;
	}

	public static float ClampMin(float val, float min)
	{
		return Math.Max(val, min);
	}

	public static float ClampMax(float val, float max)
	{
		return Math.Min(val, max);
	}

	public static float ClampRange(float val, float min, float max)
	{
		return ClampMin(ClampMax(val, max), min);
	}

	public static int ClampMin(int val, int min)
	{
		return Math.Max(val, min);
	}

	public static int ClampMax(int val, int max)
	{
		return Math.Min(val, max);
	}

	public static int ClampRange(int val, int min, int max)
	{
		return ClampMin(ClampMax(val, max), min);
	}

	public static T[] RemoveDuplicates<T>(T[] objArr)
	{
		return objArr.Distinct().ToArray();
	}

	public static void PrintAll<T>(T[] objArr, Separator separator)
	{
		string seperatorStr;

		switch (separator)
		{
			case Seperator.Comma:
				seperatorStr = ", ";
				break;
			case Seperator.Space:
				seperatorStr = " ";
				break;
			case Seperator.NewLine:
				seperatorStr = "\n";
				break;
			default:
				seperatorStr = ", ";
				break;
		}

		for (int i = 0; i < objArr.Length-1; i++)
		{
			Console.Write(objArr[i] + seperatorStr);
		}

		Console.WriteLine(objArr[objArr.Length-1]);
	}
}
enum Separator
{
	Comma,
	Space,
	NewLine,
}