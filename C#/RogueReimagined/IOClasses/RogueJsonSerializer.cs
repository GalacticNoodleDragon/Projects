using Rogue_Reimagined.EntitiesDto;
using Rogue_Reimagined.IOClasses.Interfaces;
using Rogue_Reimagined.Entities;
using Rogue_Reimagined.DtoClasses;
using Rogue_Reimagined.MiscClasses;

using System.Text.Json;

namespace Rogue_Reimagined.IOClasses;

public class RogueJsonSerializer : IRogueJsonSerializer
{
    private readonly string _savedPath = $"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName}\\Saved\\";
    private readonly List<Entity> _entities = [];
    private readonly Player? _player;

    public RogueJsonSerializer() {}

    public RogueJsonSerializer(List<Entity> entities, Player player)
    {
        _entities = new(entities);
        _player = player;
    }

    public List<Entity> Deserialize(string saveName)
    {
        string path = _savedPath + saveName + ".json";
        string jsonString = File.ReadAllText(path);
        List<EntityDto>? entities = JsonSerializer.Deserialize<List<EntityDto>>(jsonString);

        path = _savedPath + saveName + "Inventory.json";
        jsonString = File.ReadAllText(path);
        InventoryDto? inventory = JsonSerializer.Deserialize<InventoryDto>(jsonString);
        return ConvertToEntities(entities, inventory);
    }

    public async Task SerializeAsync(string saveName)
    {
        using (StreamWriter file = new(_savedPath + saveName + ".json"))
        {
            List<EntityDto> entitiesDtos = ConvertFromEntities(_entities);

            JsonSerializerOptions options = new() { WriteIndented = true };
            string json = JsonSerializer.Serialize(entitiesDtos, options);
            await file.WriteAsync(json);
        }

        using (StreamWriter file = new(_savedPath + saveName + "Inventory.json"))
        {
            InventoryDto inventoryDto = ConvertFromInventory(_player.Inventory);

            JsonSerializerOptions options = new() { WriteIndented = true };
            string json = JsonSerializer.Serialize(inventoryDto, options);
            await file.WriteAsync(json);
        }
    }

    public InventoryDto ConvertFromInventory(Inventory inventory)
    {
        Item weapon = inventory.EquipedWeapon;
        Item armor = inventory.EquipedArmor;
        List<Item> items = inventory.CollectedItems;

        ItemDto weaponDto = new(weapon.Name, weapon.Type, weapon.Value);
        ItemDto armorDto = new(armor.Name, armor.Type, armor.Value);
        List<ItemDto> itemsDto = [];

        foreach (Item item in items)
            itemsDto.Add(new(item.Name, item.Type, item.Value));

        InventoryDto retval = new(itemsDto, weaponDto, armorDto);
        return retval;
    }

    public Inventory ConvertToInventory(InventoryDto inventoryDto)
    {
        ItemDto weaponDto = inventoryDto.EquippedWeapon;
        ItemDto armorDto = inventoryDto.EquippedArmor;
        List<ItemDto> itemsDto = inventoryDto.Items;

        Item weapon = new(weaponDto.Name, weaponDto.Type, weaponDto.Value);
        Item armor = new(armorDto.Name, armorDto.Type, armorDto.Value);
        List<Item> items = [];

        foreach (ItemDto itemDto in itemsDto)
            items.Add(new(itemDto.Name, itemDto.Type, itemDto.Value));
        
        Inventory retval = new(items, weapon, armor);
        return retval;
    }

    public List<Entity> ConvertToEntities(List<EntityDto>? entities, InventoryDto? inventory)
    {
        List<Entity> retval = [];

        foreach (EntityDto entity in entities)
        {
            switch (entity.EntityType)
            {
                case "Enemy":
                    {
                        Enemy enemy = new('E', new(entity.PositionX, entity.PositionY), entity.MaxHealtPoints, entity.Attack);
                        enemy.HealthPoints = entity.HealthPoints;
                        enemy.IsVisible = entity.IsVisible;
                        retval.Add(enemy);
                        break;
                    };
                case "Exit":
                    {
                        Exit exit = new('%', new(entity.PositionX, entity.PositionY));
                        retval.Add(exit);
                        exit.IsVisible = entity.IsVisible;
                        break;
                    }
                case "Item":
                    {
                        Pickable item = new('!', new(entity.PositionX, entity.PositionY));
                        retval.Add(item);
                        item.IsVisible = entity.IsVisible;
                        break;
                    }
                case "Player":
                    {
                        Player player = new('@', new(entity.PositionX, entity.PositionY), entity.MaxHealtPoints, entity.Attack, entity.CurrentFloor);
                        player.HealthPoints = entity.HealthPoints;
                        player.Experience = entity.Experience;
                        player.NextLevelXP = entity.NextLevelExperience;
                        player.Level = entity.Level;
                        player.Inventory = ConvertToInventory(inventory);
                        player.IsVisible = entity.IsVisible;
                        retval.Add(player);
                        break;
                    }
                case "Wall":
                    {
                        Wall wall = new('#', new(entity.PositionX, entity.PositionY));
                        retval.Add(wall);
                        wall.IsVisible = entity.IsVisible;
                        break;
                    }
                default: break;
            }
        }

        return retval;
    }

    public List<EntityDto> ConvertFromEntities(List<Entity> entities)
    {
        List<EntityDto> retval = [];

        foreach (Entity entity in entities)
        {
            EntityDto entityDto = new();
            entityDto.MapChar = entity.MapChar;
            entityDto.PositionX = entity.Position.X;
            entityDto.PositionY = entity.Position.Y;
            entityDto.IsVisible = entity.IsVisible;

            if (entity.GetType() == typeof(Enemy))
            {
                Enemy enemy = (Enemy) entity;
                entityDto.EntityType = "Enemy";
                entityDto.Attack = enemy.Attack;
                entityDto.HealthPoints = enemy.HealthPoints;
                entityDto.MaxHealtPoints = enemy.MaxHealthPoints;
            }
            else if (entity.GetType() == typeof(Player))
            {
                Player player = (Player) entity;
                entityDto.EntityType = "Player";
                entityDto.Attack = player.Attack;
                entityDto.HealthPoints = player.HealthPoints;
                entityDto.MaxHealtPoints = player.MaxHealtPoints;
                entityDto.Experience = player.Experience;
                entityDto.NextLevelExperience = player.NextLevelXP;
                entityDto.Level = player.Level;
                entityDto.CurrentFloor = player.CurrentFloor;
            }
            else if (entity.GetType() == typeof(Exit))
            {
                entityDto.EntityType = "Exit";
            }
            else if (entity.GetType() == typeof(Pickable))
            {
                entityDto.EntityType = "Item";
            }
            else if (entity.GetType() == typeof(Wall))
            {
                entityDto.EntityType = "Wall";
            }

            retval.Add(entityDto);
        }

        return retval;
    }
}
