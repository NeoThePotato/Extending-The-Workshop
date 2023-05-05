namespace Combat
{

	class Unit
	{
		// Stats
		private string _name;
		private readonly int _maxHP;
		private int _currentHP;
		private float _evasion;

		// Inventory
		private int _healingPotions;
		private Weapon _weapon;
		private Shield _shield;

		public Unit(string name, int HP, float evasion, int healingPotions, Weapon weapon, Shield shield)
		{
			_name = name;
			_maxHP = HP;
			_currentHP = HP;
			_evasion = evasion;
			_healingPotions = healingPotions;
			_weapon = weapon;
			_shield = shield;
		}

		public string TakeDamage(int damage)
		{
			if (!AttemptDodge())
			{
				int unblockedDamage = _shield.GetUnblockedDamage(damage);
				SetHP(_currentHP - unblockedDamage);
				return "dealt " + unblockedDamage + " damage. They have " + _currentHP + "/" + _maxHP + " HP left";
			}
			return this + " evaded the attack";

		}

		private void Heal(int heal)
		{
			SetHP(_currentHP + heal);
		}

		public bool IsDead()
		{
			return _currentHP == 0;
		}

		private void SetHP(int HP)
		{
			_currentHP = Math.Max(0, Math.Min(HP, _maxHP));
		}

		private bool AttemptDodge()
		{
			// True for dodge
			// False for hit
			return Random.Shared.NextDouble() <= _evasion;
		}

		private bool ShouldDrinkPotion()
		{
			return _currentHP <= _maxHP / 2 && _healingPotions > 0;
		}

		public string Act(Unit enemy)
		{
			string retStr;

			if (!IsDead())
			{
				if (ShouldDrinkPotion())
					retStr = ActDrinkPotion();
				else
					retStr = ActAttack(enemy);
			}
			else
			{
				retStr = " is dead";
			}

			return this + retStr + ".";

		}

		private string ActAttack(Unit enemy)
		{
			return " attacked " + enemy + " and " + _weapon.Attack(enemy);
		}

		private string ActDrinkPotion()
		{
			if (_healingPotions > 0)
			{
				int prevHP = _currentHP;
				int healAmount = _maxHP / 2;
				Heal(healAmount);
				_healingPotions--;
				int recoveredHP = _currentHP - prevHP;

				return " drank a health potion and recovered " + recoveredHP + " HP. They have " + _healingPotions + " potions left";
			}

			return " reached for his inventory to heal but had no potions left";
		}

		public string GetStats()
		{
			return this + "\nHP: " + _currentHP + "/" + _maxHP + ".\nEvasion: " + _evasion + "%.\nPotions: " + _healingPotions + ".\nWeapon: " + _weapon.GetStats() + "\nShield: " + _shield.GetStats();
		}

		public override string ToString()
		{
			return _name;
		}

		public static void Deathmatch(Unit unit1, Unit unit2)
		{
			Console.WriteLine(DeathmatchHeader(unit1, unit2));
			Unit actingUnit = unit1, defendingUnit = unit2;

			while (!defendingUnit.IsDead())
			{
				Console.WriteLine(actingUnit.Act(defendingUnit));
				Swap(ref actingUnit, ref defendingUnit);
				Thread.Sleep(500);
			}
			Console.WriteLine(actingUnit + " wins!");
		}

		private static string DeathmatchHeader(Unit unit1, Unit unit2)
		{
			return unit1.GetStats() + "\n\nVS\n\n" + unit2.GetStats() + "\n\nBegin!";
		}

		public static void Swap(ref Unit unit1, ref Unit unit2)
		{
			Unit temp = unit1;
			unit1 = unit2;
			unit2 = temp;
		}
	}

}
