using System.Text;

namespace Visuals.Render
{

	/// <summary>
	/// Handles rendering of elements into the console
	/// </summary>
	class Renderer
	{
		private char[,] _bufferCache;
		private char[,] BufferCache {
			get => _bufferCache;
			set => _bufferCache = value;
		}
		private Element[] Elements { get; set; }

		public Renderer(int elementsCount, int dimI, int dimJ)
		{
			BufferCache = new char[dimI, dimJ];
			Elements = new Element[elementsCount];
		}

		private void CopyFrom(Element element)
		{
			CopyFrom(element.renderable.Render(), ref _bufferCache, element.offsetI, element.offsetJ);
		}

		private static void CopyFrom(char[,] source, ref char[,] destination, int offsetI, int offsetJ)
		{
			for (int j = 0; j < source.GetLength(1); j++) // Row iteration
			{
				for (int i = 0; i < source.GetLength(0); i++) // Char iteration
				{
					destination[i+offsetI, j+offsetJ] = source[i, j];
				}
			}
		}

		private void FillBuffer()
		{
			foreach (var element in Elements)
				CopyFrom(element);
		}
	}

	/// <summary>
	/// Contains a renderable element with info of where it should be rendered on the screen-space
	/// </summary>
	struct Element
	{
		public bool enabled;
		public int offsetI, offsetJ;
		public IRenderable renderable;
	}

}
