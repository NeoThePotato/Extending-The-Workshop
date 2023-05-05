namespace Combat
{
	
	class Weapon
	{
		private string _name;
		private readonly int _damage;
		private readonly float _accuracy;

		public Weapon(string name, int damage, float accuracy)
		{
			_name = name;
			_damage = damage;
			_accuracy = accuracy;
		}

		public string Attack(Unit enemy)
		{
			if (AttemptHit())
				return enemy.TakeDamage(_damage);
			return "missed";
		}

		private bool AttemptHit()
		{
			// True for hit
			// False for miss
			return Random.Shared.NextDouble() <= _accuracy;
		}

		public string GetStats()
		{
			return this + ".\nAttack: " + _damage + ".\nAccuracy: " + _accuracy + "%.";
		}

		public override string ToString()
		{
			return _name;
		}
	}

}
