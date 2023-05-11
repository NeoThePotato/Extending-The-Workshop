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

		private int SizeI { get => BufferCache.GetLength(1); }
		private int SizeJ { get => BufferCache.GetLength(0); }

		public Renderer(int elementsCount, int dimI, int dimJ)
		{
			BufferCache = new char[dimJ, dimI];
			Elements = new Element[elementsCount];
		}

		public void AddElement(Element element)
		{
			Elements[0] = element; // TODO Remember this is a test
		}

		public void Render()
		{
			UpdateBuffer();
			Console.CursorVisible = false;
			StringBuilder sb = new(SizeI * SizeJ);

			for (int j = 0; j < SizeJ; j++)
			{
				for (int i = 0; i < SizeI; i++)
				{
					char c = BufferCache[j, i];
					sb.Append(c == 0? ' ' : c);
				}
				sb.Append('\n');
			}
			Console.SetCursorPosition(0, 0);
			Console.Write(sb);
		}

		private void CopyFrom(Element element)
		{
			CopyFrom(element.renderable.Render(), ref _bufferCache, element.offsetI, element.offsetJ);
		}

		private static void CopyFrom(char[,] source, ref char[,] destination, int offsetI, int offsetJ)
		{
			for (int j = 0; j < source.GetLength(0); j++) // Row iteration
			{
				for (int i = 0; i < source.GetLength(1); i++) // Char iteration
				{
					destination[j + offsetJ, i + offsetI] = source[j, i];
				}
			}
		}

		private void UpdateBuffer()
		{
			foreach (var element in Elements)
			{
				if (element.enabled)
					CopyFrom(element);
			}
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
