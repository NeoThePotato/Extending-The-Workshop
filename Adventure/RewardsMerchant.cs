namespace Adventure
{
	class RewardsMerchant
	{
		public int Level { get; private set; } = 0;

		public RewardsMerchant()
		{

		}

        public void RaiseLevel()
        {
			RaiseLevel(1);
        }

        public void RaiseLevel(int amount)
		{
			Level += amount;
		}
	}
}
