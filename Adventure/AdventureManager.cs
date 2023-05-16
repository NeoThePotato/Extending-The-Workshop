using Combat;
using System;
using System.Text;
using Templates;

namespace Adventure
{
	class AdventureManager
	{

		private Unit _playerUnit;
		private Unit[] _enemyPool;
		private int EnemiesKilled
		{ get; set; }

		private const int ENEMY_SELECTION_SIZE = 3;

		public AdventureManager()
		{
			_playerUnit = Units.hero;
			_enemyPool = new Unit[] { Units.slime, Units.fae, Units.annoyingFly, Units.imp, Units.spawnOfTwilight, Units.antiHero, Units.invincibleArchdemon};
		}

		public void Adventure()
		{
			AdventureStart();
			AdventureLoop();
			AdventureEnd();
		}

		private void AdventureStart()
		{
			Console.Clear();
			Console.WriteLine($"Welcome, {_playerUnit.Name}.\nAre you ready to begin your adventure?\n(Press any key to continue...)");
			Utility.BlockUntilKeyDown();
		}

		private void AdventureLoop()
		{
			// Some would call this the "Core gameplay loop" :)
			while (FindEncounter())
			{
				// TODO Used RewardsMerchant here
			}
		}

		private void AdventureEnd()
		{
			Console.WriteLine($"\nAnd so end {_playerUnit.Name}'s adventure.\nThey have felled {EnemiesKilled} enemies along the way.");
		}

		private bool FindEncounter()
		{
			Console.Clear();
			Console.WriteLine("You see some enemies in the distance.\nWho would you like to engage?");
			Unit enemy = PickEnemyScreen();
			Unit winner = EnterCombat(enemy);
			bool playerWins = winner == _playerUnit;

			if (playerWins)
				EnemiesKilled++;
			
			return playerWins;
		}

		private Unit EnterCombat(Unit enemy)
		{
			return new CombatManager(_playerUnit, enemy).Combat();
		}

		private Unit PickEnemyScreen()
		{
			Unit[] enemies = GetEnemySelection(ENEMY_SELECTION_SIZE);
			PrintEnemySelection(enemies);

			return new Unit(GetEnemySelectionPlayerInput(enemies));

		}

		private void PrintEnemySelection(Unit[] enemies)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < enemies.Length; i++)
				stringBuilder.Append($"{i+1}: {enemies[i]}\n");
			stringBuilder.Append($"{enemies.Length}+: ???\n");
			Console.Write(stringBuilder.ToString());
		}

		private Unit GetEnemySelectionPlayerInput(Unit[] enemies)
		{
			int input;
			while (true)
			{
				if (!int.TryParse(Console.ReadLine(), out input) || 1 > input)
				{
					(int left, int top) = Console.GetCursorPosition();
					Console.SetCursorPosition(left, top - 1);
					continue;
				}
				if (input > enemies.Length)
					return GetRandomEnemy();
				else
					return enemies[input-1];
			}
		}

		private Unit[] GetEnemySelection(int num)
		{
			Unit[] units = new Unit[num];

			for (int i = 0; i < num; i++)
				units[i] = GetRandomEnemy();

			return Utility.RemoveDuplicates(units);
		}

		private Unit GetRandomEnemy()
		{
			return _enemyPool[Random.Shared.Next(0, _enemyPool.Length)];
		}

	}

}
