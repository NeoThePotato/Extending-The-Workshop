namespace Visuals.Render
{

	/// <summary>
	/// Handles rendering of elements into the console
	/// </summary>
	class Renderer
	{
		private char[,] BufferCache { get; set; }
		private Element[] Elements { get; set; }

		public Renderer(int elementsCount, int dimX, int dimY)
		{
			BufferCache = new char[dimX, dimY];
			Elements = new Element[elementsCount];
		}

		public void Render() { }
	}

	/// <summary>
	/// Contains a renderable element with info of where it should be rendered on the screen-space
	/// </summary>
	struct Element
	{
		public int x, y;
		public IRenderable renderable;
	}

}
