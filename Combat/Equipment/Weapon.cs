namespace Combat.Equipment
{
	
	class Weapon
	{
		public string Name { get; private set; }
		public int Damage { get; private set; }

		public Weapon(string name, int damage)
		{
			Name = name;
			Damage = damage;
		}

		public string GetStats()
		{
			return $"{this}.\nDamage:{Damage}.";
		}

		public override string ToString()
		{
			return Name;
		}
	}

}
