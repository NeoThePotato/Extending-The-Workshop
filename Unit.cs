namespace Combat
{

	class Unit
	{
		// Stats
		public string Name { get; private set; }
		public int MaxHP {
			get
			{
				return MaxHP;
			}

			private set
			{
				int modifyMaxHP = value - MaxHP;
				int increaseCurrentHP = Math.Max(0, modifyMaxHP);
				MaxHP += modifyMaxHP;
				CurrentHP += increaseCurrentHP;
			}
		}
		public int CurrentHP {
			get
			{
				return CurrentHP;
			}

			private set
			{
				CurrentHP = Math.Max(0, Math.Min(value, MaxHP));
			}
		}
		public bool Dead { get => CurrentHP == 0; }

		// Equipment
		private Weapon _weapon;
		private Shield _shield;

		public Unit(string name, int HP, Weapon weapon, Shield shield)
		{
			Name = name;
			MaxHP = HP;
			CurrentHP = MaxHP;
			_weapon = weapon;
			_shield = shield;
		}

		public void TakeDamage(int damage)
		{
			int unblockedDamage = _shield.GetUnblockedDamage(damage);
			CurrentHP -= unblockedDamage;
		}

		public void Heal(int heal)
		{
			CurrentHP += heal;
		}

		public void Attack(Unit enemy)
		{
			enemy.TakeDamage(_weapon.Damage);
		}

		public string GetStats()
		{
			return $"{this}\nHP: {CurrentHP}/{MaxHP}.\nWeapon: {_weapon.GetStats()}\nShield: {_shield.GetStats()}";
		}

		public override string ToString()
		{
			return Name;
		}

	}

}
