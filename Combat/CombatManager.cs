namespace Combat
{
	class CombatManager
	{

		private Unit PlayerUnit;
		private Unit CPUUnit;

		private bool BothAlive
		{
			get => !PlayerUnit.Dead && !CPUUnit.Dead;
		}

		public CombatManager(Unit playerUnit, Unit cpuUnit)
		{
			PlayerUnit = playerUnit;
			CPUUnit = cpuUnit;
		}

		public void CombatInitiate()
		{
			PlayerUnit.ResetTempStats();
			CPUUnit.ResetTempStats();
			Console.WriteLine($"{PlayerUnit} has encountered a {CPUUnit}.");
			BlockUntilKeyDown();
		}

		public Unit CombatLoop()
		{
			Unit actingUnit = PlayerUnit;
			Unit passiveUnit = CPUUnit;
			CombatFeedback combatFeedback = new CombatFeedback();

			while (BothAlive)
			{
				Console.Clear();
				Console.WriteLine(
					$"{PlayerUnit.GetCombatStats()}\n\n" +
					$"{CPUUnit.GetCombatStats()}\n\n" +
					$"{actingUnit}'s turn.\n");

				if (actingUnit == PlayerUnit)
					PlayerUnitAct(ref combatFeedback);
				else
					CPUUnitAct(ref combatFeedback);

				Console.WriteLine(combatFeedback.ParseFeedback());
				BlockUntilKeyDown();
				Utility.Swap(ref actingUnit, ref passiveUnit);
			}

			Unit winner = GetWinner();
			Console.WriteLine($"{winner} wins!");

			return winner;
		}

		private void PlayerUnitAct(ref CombatFeedback feedback)
		{
			Console.WriteLine(
				$"What will {PlayerUnit} do?\n" +
				$"{GetPlayerOptions()}");

			switch (GetPlayerInput())
			{
				case 1:
					PlayerUnit.AttackOther(CPUUnit, ref feedback);
					break;
				case 2:
					PlayerUnit.RaiseShield(ref feedback);
					break;
				case 3:
					PlayerUnit.HealSelf(ref feedback);
					break;
			}

		}

		private void CPUUnitAct(ref CombatFeedback feedback)
		{
			Thread.Sleep(300);
			CPUUnit.AttackOther(PlayerUnit, ref feedback);
		}

		private void BlockUntilKeyDown()
		{
			Console.ReadKey();
		}

		private string GetPlayerOptions()
		{
			return	("1: Attack\n" +
					"2: Defend\n" +
					"3: Heal");
		}

		private int GetPlayerInput()
		{
			int input = 0;
			while(1 > input || input > 3)
				while (!int.TryParse(Console.ReadLine(), out input));

			return input;
		}

		private Unit GetWinner()
		{
			return CPUUnit.Dead ? PlayerUnit : CPUUnit;
		}

	}
}
