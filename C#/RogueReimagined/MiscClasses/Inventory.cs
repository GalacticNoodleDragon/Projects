namespace Rogue_Reimagined.MiscClasses;

public class Inventory
{
    public List<Item> CollectedItems { get; set; } = [];
    public Item EquipedWeapon { get; set; } = new("Dagger", "weapon", 0);
    public Item EquipedArmor { get; set; } = new("Clothes", "armor", 0);

    public Inventory() {}

    public Inventory(List<Item> items, Item weapon, Item armor)
    {
        CollectedItems = items;
        EquipedWeapon = weapon;
        EquipedArmor = armor;
    }
}
