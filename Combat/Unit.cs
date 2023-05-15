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
                int increaseCurrentHP = Math.Max(0, modifyMaxHP);
				_maxHP += modifyMaxHP;
                CurrentHP += increaseCurrentHP;
            }
        }
        public int Strength
        {
            get
            {
                return _strength;
            }

            private set
            {
				_strength = Math.Max(0, value);
            }
        }
        public float Evasion
		{
			get => _evasion;
			private set => _evasion = value;
		}
		public int CurrentHP
		{
			get
			{
				return _currentHP;
			}

			private set
			{
				_currentHP = Math.Max(0, Math.Min(value, MaxHP));
			}
		}
		public float HealingPower
        {
            get => _healingPower;
			private set => _healingPower = value;
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
            get => _equippedWeapon == null? Templates.Weapons.nothing : _equippedWeapon;
            set => _equippedWeapon = value;
		}
		public Armor Shield
		{
			get => _equippedShield == null ? Templates.Armors.nothing : _equippedShield;
			set => _equippedShield = value;
		}
		public Armor BodyArmor
		{
			get => _equippedBodyArmor == null ? Templates.Armors.nothing : _equippedBodyArmor;
			set => _equippedBodyArmor = value;
		}

		public Unit(string name, int HP, int strength, float evasion, Weapon weapon, Armor shield, Armor bodyArmor)
        {
            Name = name;
            MaxHP = HP;
            Strength = strength;
            Evasion = evasion;
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
            HealingPower *= 0.5f;
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

        public string GetStats()
        {
            return $"{this}" +
                $"\nHP: {CurrentHP}/{MaxHP}" +
                $"\nStrength: {Strength}" +
                $"\nEvasion: {Evasion}" +
                $"\nWeapon: {Weapon.GetStats()}" +
                $"\nShield: {Shield.GetStats()}" +
                $"\nBody Armor: {BodyArmor.GetStats()}" +
                $"\nHealing Power: {EffectiveHealPower} ({HealingPower*100f}%)";
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
				feedback.numericAmount = GetUnblockedDamage(damage);
				CurrentHP -= GetUnblockedDamage(damage);
			}
			else
			{
				feedback.type = CombatFeedback.FeedbackType.Evade;
				feedback.numericAmount = 0;
			}
			Blocking = false;
		}

		private void HealBy(int heal)
		{
			CurrentHP += heal;
		}

        private void ResetTempStats()
        {
            CurrentHP = MaxHP;
            HealingPower = 1f;
            Blocking = false;
        }

		private int GetUnblockedDamage(int damage)
        {
            return Math.Max(1, damage - EffectiveDefense);
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
				case FeedbackType.Block:    return $"{actor}'s attack was blocked {other} but dealt {numericAmount} damage.";
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
			    Heal,
		    }
	}

}
