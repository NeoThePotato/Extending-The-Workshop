using Combat.Equipment;
using Templates;

namespace Combat
{

    class Unit
    {
		// Stats
        private string _name;
		private int _maxHP;
        private int _currentHP;
        private int _strength;
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
        public bool Blocking
        {
            get;
            set;
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

		public Unit(string name, int HP, int strength, Weapon weapon, Armor shield, Armor bodyArmor)
        {
            Name = name;
            MaxHP = HP;
            CurrentHP = MaxHP;
            Strength = strength;
            Weapon = weapon;
            Shield = shield;
            BodyArmor = bodyArmor;
        }

        public void AttackOther(Unit other)
        {
            other.TakeDamage(EffectiveAttack);
        }

        public void TakeDamage(int damage)
        {
            CurrentHP -= GetUnblockedDamage(damage);
        }

        public void Heal(int heal)
        {
            CurrentHP += heal;
        }

        public string GetStats()
        {
            return $"{this}\nHP: {CurrentHP}/{MaxHP}.\nWeapon: {Weapon.GetStats()}\nShield: {Shield.GetStats()}\nBody Armor: {BodyArmor.GetStats()}";
        }

        public override string ToString()
        {
            return Name;
        }

        private int GetUnblockedDamage(int damage)
        {
            return Math.Max(1, damage - EffectiveDefense);
        }

    }

}
