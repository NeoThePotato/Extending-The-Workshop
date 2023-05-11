namespace Combat
{

	class Grid
	{

		private Unit[,] UnitGrid { get; set; }

		public int GridSize { get => UnitGrid.GetLength(0); }

		public int GridArea { get => UnitGrid.Length; }

		public bool GridFull { get => UnitsCount == GridArea; }

		public int UnitsCount
		{
			get
			{
				int unitsCount = 0;

				for (int j = 0; j < GridSize; j++)
				{
					for (int i = 0; i < GridSize; i++)
					{
						unitsCount += UnitExists(j, i) ? 1 : 0;
					}
				}

				return unitsCount;
			}
		}

		public static readonly (int, int) INVALID_POSITION = (-1, -1);

		public Grid(int size)
		{
			UnitGrid = new Unit[size, size];
		}

		public Unit GetUnitAt(int posJ, int posI)
		{
			return UnitGrid[posJ, posI];
		}

		public bool UnitExists(int posJ, int posI)
		{
			return GetUnitAt(posJ, posI) != null;
		}

		private (int, int) GetEmptySpace()
		{
			if (!GridFull)
			{
				return INVALID_POSITION;
			}
			else
			{
				for (int j = 0; j < GridSize; j++)
				{
					for (int i = 0;  i < GridSize; i++)
					{
						if (!UnitExists(j, i))
						{
							return (j, i);
						}
					}
				}
			}

			return INVALID_POSITION;
		}

		public void AddUnit(Unit unit)
		{
			var emptySpace = GetEmptySpace();

			if (emptySpace != INVALID_POSITION)
			{
				AddUnit(unit, emptySpace.Item1, emptySpace.Item2);
			}
		}

		private void AddUnit(Unit unit, int posJ, int posI)
		{
			if (GridFull)
			{
				throw new InvalidOperationException("Can't add another unit, grid is full.");
			}
			else if (UnitExists(posJ, posI))
			{
				throw new InvalidOperationException("Can't add unit to occupied space.");
			}
			else
			{
				UnitGrid[posJ, posI] = unit;
			}
		}

		public enum Direction
		{
			Back	= 0b0001,
			Front	= 0b0010,
			Left	= 0b0100,
			Right	= 0b1000,
		}

	}

}
