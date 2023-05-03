namespace Visuals.Render
{

	/// <summary>
	/// Interface which can be rendered into a Renderer
	/// </summary>
	interface IRenderable
	{
		public char[,] Render();
	}
}
