namespace Rogue_Reimagined.DtoClasses;

public class InventoryDto(List<ItemDto> items, ItemDto equippedWeapon, ItemDto equippedArmor)
{
    public List<ItemDto> Items { get; set; } = items;
    public ItemDto EquippedWeapon { get; set; } = equippedWeapon;
    public ItemDto EquippedArmor { get; set; } = equippedArmor;
}
