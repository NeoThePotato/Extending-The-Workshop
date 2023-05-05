namespace Combat
{

	class Shield
	{
		private string _name;
		private int _shield;

		public Shield(string name, int shield)
		{
			_name = name;
			_shield = shield;
		}

		public int GetShield()
		{
			return _shield;
		}

		public int GetUnblockedDamage(int damage)
		{
			return Math.Max(0, damage - GetShield());
		}

		public void UpgradeShield()
		{
			_shield++;
		}

		public string GetStats()
		{
			return this + ".\nDefense: " + _shield + ".";
		}

		public override string ToString()
		{
			return _name;
		}
	}

}
