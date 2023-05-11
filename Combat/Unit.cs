using Combat.Equipment;

namespace Combat
{

    class Unit
    {
        // Stats
        public string Name { get; private set; }
        public int MaxHP
        {
            get
            {
                return MaxHP;
            }

            private set
            {
                int modifyMaxHP = value - MaxHP;
                int increaseCurrentHP = Math.Max(0, modifyMaxHP);
                MaxHP += modifyMaxHP;
                CurrentHP += increaseCurrentHP;
            }
        }
        public int CurrentHP
        {
            get
            {
                return CurrentHP;
            }

            private set
            {
                CurrentHP = Math.Max(0, Math.Min(value, MaxHP));
            }
        }
        public int Strength
        {
            get
            {
                return Strength;
            }

            private set
            {
                Strength = Math.Max(0, value);
            }
        }
        public bool Blocking { get; set; }
        public bool Dead
        {
            get => CurrentHP == 0;
        }
        private int EffectiveAttack
        {
            get => Strength + _weapon.Damage;
        }
        private int EffectiveDefense
        {
            get => _bodyArmor.Defense + (Blocking ? _shield.Defense : 0);
        }

        // Equipment
        private Weapon _weapon;
        private Armor _shield;
        private Armor _bodyArmor;

        public Unit(string name, int HP, Weapon weapon, Armor shield, Armor armor)
        {
            Name = name;
            MaxHP = HP;
            CurrentHP = MaxHP;
            _weapon = weapon;
            _shield = shield;
            _bodyArmor = armor;
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
            return $"{this}\nHP: {CurrentHP}/{MaxHP}.\nWeapon: {_weapon.GetStats()}\nShield: {_shield.GetStats()}\nBody Armor: {_bodyArmor.GetStats()}";
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
