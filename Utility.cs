static class Utility
{
	public static void Swap<T>(ref T obj1, ref T obj2)
	{
		var temp = obj1;
		obj1 = obj2;
		obj2 = temp;
	}

	public static T[] RemoveDuplicates<T>(T[] objArr)
	{
		return objArr.Distinct().ToArray();
	}

	public static void PrintAll<T>(T[] objArr, Seperator seperator)
	{
		string seperatorStr;

		switch (seperator)
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
enum Seperator
{
	Comma,
	Space,
	NewLine,
}