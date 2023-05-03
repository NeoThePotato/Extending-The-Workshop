using Visuals.Render;

namespace Test {

	class Bob : IRenderable
	{
		private readonly char[,] _bob = {
		{' ', 'o', ' '},
		{' ', '|', ' '},
		{'-', 'O', '-'},
		{' ', '|', ' '},
		{'/', ' ', '\\'}
		};
		public int SizeI { get => _bob.GetLength(1); }
		public int SizeJ { get => _bob.GetLength(0); }

		public char[,] Render()
		{
			return _bob;
		}
	}

	public static class Methods
	{

		public static void TestBob()
		{
			Element bobElement = new Element();
			bobElement.enabled = true;
			bobElement.offsetI = 0;
			bobElement.offsetJ = 0;
			bobElement.renderable = new Bob();

			Renderer renderer = new Renderer(1, 10, 10);
			renderer.AddElement(bobElement);
			renderer.Render();

			char[,] _bob = {
			{' ', 'o', ' '},
			{' ', '|', ' '},
			{'-', 'O', '-'},
			{' ', '|', ' '},
			{'/', ' ', '\\'}
			};
			Console.WriteLine(_bob.GetLength(1));

		}
	}
}