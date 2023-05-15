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

		public void CombatLoop(out Unit winner)
		{
			CombatFeedback combatFeedback = new CombatFeedback();

			Unit actingUnit = PlayerUnit;
			Unit passiveUnit = CPUUnit;
			while (BothAlive)
			{
				if (actingUnit == PlayerUnit)
					PlayerUnitAct(ref combatFeedback);
				else
					CPUUnitAct(ref combatFeedback);

				Console.WriteLine(combatFeedback.ParseFeedback());

				if (passiveUnit.Dead)
				{
					Console.WriteLine($"{actingUnit} has defeated {passiveUnit}!");
					winner = actingUnit;
					break;
				}
			}
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

	}
}
