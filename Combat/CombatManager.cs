using Combat.Equipment;
using System.Text;

namespace Combat
{
	class CombatManager
	{

		public const int GRID_SIZE = 3;

		Unit[] PlayerUnits;
		Unit[] CPUUnits;
		Grid PlayerGrid { get; set; }
		Grid CPUGrid { get; set; }

		public CombatManager(Unit[] playerUnits, Unit[] cpuUnits)
		{
			PlayerUnits = playerUnits;
			CPUUnits = cpuUnits;
			PlayerGrid = new Grid(GRID_SIZE);
			CPUGrid = new Grid(GRID_SIZE);

			foreach (Unit unit in PlayerUnits)
			{
				PlayerGrid.AddUnit(unit);
			}

			foreach (Unit unit in CPUUnits)
			{
				CPUGrid.AddUnit(unit);
			}
		}

		public void CombatLoop()
		{

			while (true) // TODO Implement
			{
				// Player turn
				foreach (Unit unit in PlayerUnits)
					PlayerUnitAct(unit);

				if (AllDead(CPUUnits))
					break;

				// CPU turn
				foreach (Unit unit in CPUUnits)
					CPUUnitAct(unit);

				if (AllDead(PlayerUnits))
					break;
			}
		}

		private void PlayerUnitAct(Unit unit)
		{
			unit.Blocking = false;
			Console.WriteLine($"{unit} is acting.");
			Console.WriteLine($"What will {unit} do?");

		}

		private void CPUUnitAct(Unit unit)
		{
			if (unit.Dead)
			{
				return;
			}
			else
			{
				Unit attackedPlayerUnit = PickRandomUnit(PlayerUnits);
				AttackUnit(unit, attackedPlayerUnit);
			}
		}

		private void AttackUnit(Unit attacker, Unit attacked)
		{
			int previousHP = attacked.CurrentHP;
			attacker.AttackOther(attacked);
			Console.WriteLine($"{attacker} attacked {attacked} and dealt {previousHP - attacked.CurrentHP} damage.");
		}

		private static (int, int) GetMyPosition(Unit unit, Grid grid)
		{
			throw new NotImplementedException();
		}

		private static Unit[] GetTargetableUnits(Unit unit)
		{
			throw new NotImplementedException();
		}

		private static bool AllDead(Unit[] units)
		{
			foreach (Unit unit in units)
			{
				if (!unit.Dead)
				{
					return false;
				}
			}

			return true;
		}

		private static Unit PickRandomUnit(Unit[] units)
		{
			return units[Random.Shared.Next(0, units.Length)];
		}

	}
}
