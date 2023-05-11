namespace Combat
{

	class Shield
	{
		public string Name { get; private set; }
		public int Defense { get; private set; }

		public Shield(string name, int shield)
		{
			Name = name;
			Defense = shield;
		}

		public int GetUnblockedDamage(int damage)
		{
			return Math.Max(0, damage - Defense);
		}

		public string GetStats()
		{
			return $"{this}.\nDefense: {Defense}.";
		}

		public override string ToString()
		{
			return Name;
		}
	}

}
