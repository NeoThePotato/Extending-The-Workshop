using Combat.Equipment;

namespace Templates
{

	static class Weapons
	{
		public static Weapon nothing = new Weapon("", 0);
		public static Weapon rustedBlade = new Weapon("Rusted Blade", 1);
	}

	static class Armors
	{
		public static Armor nothing = new Armor("", 0);
		public static Armor rustedChestplate = new Armor("Rusted Chestplate", 1);
		public static Armor rustedBuckler = new Armor("Rusted Buckler", 1);
	}

}
