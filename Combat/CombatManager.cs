namespace Combat
{
	class CombatManager
	{

		Unit PlayerUnit;
		Unit CPUUnit;

		private bool BothAlive
		{
			get => !PlayerUnit.Dead && !CPUUnit.Dead;
		}

		public CombatManager(Unit playerUnit, Unit cpuUnit)
		{
			PlayerUnit = playerUnit;
			CPUUnit = cpuUnit;
		}

		public Unit CombatLoop()
		{
			Unit actingUnit = PlayerUnit;
			Unit passiveUnit = CPUUnit;
			CombatFeedback combatFeedback = new CombatFeedback();

			while (BothAlive)
			{
				if (actingUnit == PlayerUnit)
					PlayerUnitAct(ref combatFeedback);
				else
					CPUUnitAct(ref combatFeedback);

				Console.WriteLine(combatFeedback.ParseFeedback());
			}

			Unit winner = GetWinner();
			Console.WriteLine($"{winner} wins!");

			return winner;
		}

		private void PlayerUnitAct(ref CombatFeedback feedback)
		{
			Console.WriteLine(
				$"What will {PlayerUnit} do?\n{PrintPlayerOptions()}");
				PrintPlayerOptions();
				// TODO Implement input

		}

		private void CPUUnitAct(ref CombatFeedback feedback)
		{
			CPUUnit.AttackOther(PlayerUnit, ref feedback);
		}

		private string PrintPlayerOptions()
		{
			// TODO Implement
			throw new NotImplementedException();
		}

		private Unit GetWinner()
		{
			return CPUUnit.Dead ? PlayerUnit : CPUUnit;
		}

	}
}
