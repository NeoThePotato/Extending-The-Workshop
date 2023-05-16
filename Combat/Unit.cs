using Combat.Equipment;
using System.Text;
using Templates;

namespace Combat
{

	class Unit
	{
		// Permanent Stats
		private string _name;
		private int _maxHP;
		private int _strength;
		private float _evasion;
		private float _initialHealingPower;
		private float _healingPowerDecay;
		// Temporary Stats
		private int _currentHP;
		private float _healingPower;
		private bool _blocking;
		// Equipment
		private Weapon? _equippedWeapon;
		private Armor? _equippedShield;
		private Armor? _equippedBodyArmor;

		public string Name
		{
			get => _name;
			private set => _name = value;
		}
		public int MaxHP
		{
			get => _maxHP;

			private set
			{
				int modifyMaxHP = value - _maxHP;
				int increaseCurrentHP = Utility.ClampMin(modifyMaxHP, 0);
				_maxHP += modifyMaxHP;
				CurrentHP += increaseCurrentHP;
			}
		}
		public int Strength
		{
			get => _strength;
			private set => _strength = Utility.ClampMin(value, 0);
		}
		public float Evasion
		{
			get => _evasion;
			private set => _evasion = Utility.ClampRange(value, 0f, 1f);
		}
		public int CurrentHP
		{
			get => _currentHP;
			private set => _currentHP = Utility.ClampRange(value, 0, MaxHP);
		}
		public float HealingPower
		{
			get => _healingPower;
			private set => _healingPower = Utility.ClampRange(value, 0f, 1f);
		}
		public float InitialHealingPower
		{
			get => _initialHealingPower;
			private set => _initialHealingPower = Utility.ClampRange(value, 0f, 1f);
		}
		public float HealingPowerDecay
		{
			get => _healingPowerDecay;
			private set => _healingPowerDecay = Utility.ClampRange(value, 0f, 1f);
		}
		public bool Blocking
		{
			get => _blocking;
			private set => _blocking = value;
		}
		public bool Dead
		{
			get => CurrentHP == 0;
		}
		private int EffectiveAttack
		{
			get => Strength + Weapon.Damage;
		}
		private int EffectiveDefense
		{
			get => BodyArmor.Defense + (Blocking ? Shield.Defense : 0);
		}
		private int EffectiveHealPower
		{
			get => (int)(HealingPower * MaxHP);
		}
		public Weapon Weapon
		{
			get => _equippedWeapon == null? Weapons.nothing : _equippedWeapon;
			set => _equippedWeapon = value;
		}
		public Armor Shield
		{
			get => _equippedShield == null ? Armors.nothing : _equippedShield;
			set => _equippedShield = value;
		}
		public Armor BodyArmor
		{
			get => _equippedBodyArmor == null ? Armors.nothing : _equippedBodyArmor;
			set => _equippedBodyArmor = value;
		}

		public Unit(string name, int HP, int strength, float evasion, float initialHealingPower, float healingPowerDecay, Weapon weapon, Armor shield, Armor bodyArmor)
		{
			Name = name;
			MaxHP = HP;
			Strength = strength;
			Evasion = evasion;
			InitialHealingPower = initialHealingPower;
			HealingPowerDecay = healingPowerDecay;
			Weapon = weapon;
			Shield = shield;
			BodyArmor = bodyArmor;
			ResetTempStats();
		}

		public void AttackOther(Unit other, ref CombatFeedback feedback)
		{
			CheckValidState();
			feedback.actor = this;
			feedback.other = other;
			other.TakeDamage(EffectiveAttack, ref feedback);
		}

		public void HealSelf(ref CombatFeedback feedback)
		{
			CheckValidState();
			int previousHP = CurrentHP;
			HealBy(EffectiveHealPower);
			ReduceHealingPower();
			feedback.actor = this;
			feedback.other = this;
			feedback.type = CombatFeedback.FeedbackType.Heal;
			feedback.numericAmount = CurrentHP - previousHP;
		}

		public void RaiseShield(ref CombatFeedback feedback)
		{
			CheckValidState();
			Blocking = true;
			feedback.actor = this;
			feedback.type = CombatFeedback.FeedbackType.Raise;
		}

		public int CalculateTotalDamageFrom(Unit attacker)
		{
			return GetUnblockedDamage(attacker.EffectiveAttack);
		}

		public void ResetTempStats()
		{
			CurrentHP = MaxHP;
			HealingPower = InitialHealingPower;
			Blocking = false;
		}

		public string GetStats()
		{
			return $"{this}" +
				$"\nHP: {CurrentHP}/{MaxHP}" +
				$"\nStrength: {Strength}" +
				$"\nEvasion: {Evasion*100f}%" +
				$"\nWeapon: {Weapon.GetStats()}" +
				$"\nShield: {Shield.GetStats()}" +
				$"\nBody Armor: {BodyArmor.GetStats()}" +
				$"\nHealing Power: {EffectiveHealPower} ({HealingPower*100f}%)";
		}

		public string GetCombatStats()
		{
			return $"{this}" +
				$"\nHP: {CurrentHP}/{MaxHP}" +
				$"\nAttack Power: {EffectiveAttack} ({Strength}+{Weapon.Damage})" +
				$"\nDefense: {EffectiveDefense} ({BodyArmor.Defense}+{(Blocking? Shield.Defense : 0)})" +
				$"\nEvasion: {Evasion * 100f}%" +
				$"\nHealing Power: {EffectiveHealPower} ({HealingPower * 100f}%)";
		}

		public override string ToString()
		{
			return Name;
		}

		private void TakeDamage(int damage, ref CombatFeedback feedback)
		{
			if (!AttemptDodge())
			{
				if (Blocking)
					feedback.type = CombatFeedback.FeedbackType.Block;
				else
					feedback.type = CombatFeedback.FeedbackType.Hit;
				int finalDamage = GetUnblockedDamage(damage);
				feedback.numericAmount = finalDamage;
				CurrentHP -= finalDamage;
				Blocking = false;
			}
			else
			{
				feedback.type = CombatFeedback.FeedbackType.Evade;
				feedback.numericAmount = 0;
			}
		}

		private void HealBy(int heal)
		{
			CurrentHP += heal;
		}

		private void ReduceHealingPower()
		{
			HealingPower *= 1f - HealingPowerDecay;
		}

		private int GetUnblockedDamage(int damage)
		{
			return Utility.ClampMin(damage - EffectiveDefense, 1);
		}

		private bool AttemptDodge()
		{
			return Evasion >= Random.Shared.NextDouble();
		}

		private void CheckValidState()
		{
			if (Dead)
				throw new InvalidOperationException($"{this} is dead and cannot act.");
		}

	}

	enum UnitAction
	{
		Attack,
		Defend,
		Heal
	}

	struct CombatFeedback
	{
		public Unit actor;
		public Unit other;
		public FeedbackType type;
		public int numericAmount;

		public string ParseFeedback()
		{
			StringBuilder sb = new StringBuilder();
			switch (type)
			{
				case FeedbackType.Hit:      return $"{actor} attacked {other} and dealt {numericAmount} damage.";
				case FeedbackType.Block:    return $"{actor}'s attack was blocked but dealt {numericAmount} damage to {other}.";
				case FeedbackType.Evade:    return $"{actor}'s attack missed {other}.";
				case FeedbackType.Raise:    return $"{actor} raised their shield.";
				case FeedbackType.Heal:     return $"{actor} healed for {numericAmount} HP.";
				default:                    return "";
			}
		}

		public enum FeedbackType
			{
				Hit,
				Block,
				Evade,
				Raise,
				Heal
			}
	}

}
