using Combat.Equipment;
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
			set => _evasion = value;
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
            set => _healingPower = value;
        }
		public bool Blocking
        {
            get => _blocking;
            set => _blocking = value;
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

        public void AttackOther(Unit other)
        {
            other.TakeDamage(EffectiveAttack);
        }

        public void TakeDamage(int damage)
        {
            CurrentHP -= GetUnblockedDamage(damage);
            Blocking = false;
        }

        public void HealSelf()
        {
            HealBy(EffectiveHealPower);
            HealingPower *= 0.5f;
        }

        public string GetStats()
        {
            return $"{this}\nHP: {CurrentHP}/{MaxHP}\nStrength: {Strength}\nEvasion: {Evasion}\nWeapon: {Weapon.GetStats()}\nShield: {Shield.GetStats()}\nBody Armor: {BodyArmor.GetStats()}";
        }

        public override string ToString()
        {
            return Name;
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

    }

}
