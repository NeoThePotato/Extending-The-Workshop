namespace Combat
{
	class CombatManager
	{

		Unit PlayerUnit;
		Unit CPUUnit;

		public CombatManager(Unit playerUnit, Unit cpuUnit)
		{
			PlayerUnit = playerUnit;
			CPUUnit = cpuUnit;
		}

		public void CombatLoop()
		{

			while (true) // TODO Implement
			{
				// Player turn
				PlayerUnitAct();

				if (CPUUnit.Dead)
					break;

				// CPU turn
				CPUUnitAct();

				if (PlayerUnit.Dead)
					break;
			}
		}

		private void PlayerUnitAct()
		{
			PlayerUnit.Blocking = false;
			Console.WriteLine(
				$"What will {PlayerUnit} do?\n"
				);

		}

		private void CPUUnitAct()
		{
			if (CPUUnit.Dead)
			{
				return;
			}
			else
			{
				AttackUnit(CPUUnit, PlayerUnit);
			}
		}

		private void PrintPlayerOptions()
		{

		}

		private void AttackUnit(Unit attacker, Unit attacked)
		{
			int previousHP = attacked.CurrentHP;
			attacker.AttackOther(attacked);
			Console.WriteLine($"{attacker} attacked {attacked} and dealt {previousHP - attacked.CurrentHP} damage.");
		}

	}
}
