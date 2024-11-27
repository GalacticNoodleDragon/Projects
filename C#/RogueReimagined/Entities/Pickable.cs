using Rogue_Reimagined.MiscClasses;

namespace Rogue_Reimagined.Entities;

public class Pickable(char mapChar, Position position) : Entity(mapChar, position)
{
    public override void Update(in EntityMap entityMap)
    {
        base.Update(entityMap);
        if (entityMap.GetPlayer().Position.Equals(Position))
        {
            int name1Idx = Random.Shared.Next(0, Constants.Names1.Count);
            int name2Idx = Random.Shared.Next(0, Constants.Names2.Count);
            string itemName = $"{Constants.Names1[name1Idx]} of {Constants.Names2[name2Idx]}";
            int itemVal;
            if (name1Idx >= 9)
                itemVal = Random.Shared.Next(10, 25) * entityMap.GetPlayer().CurrentFloor;
            else
                itemVal = Random.Shared.Next(1, 5) * entityMap.GetPlayer().CurrentFloor;

            Item newItem = new(itemName, name1Idx >= 9 ? "armor" : "weapon", itemVal);
            entityMap.GetPlayer().Inventory.CollectedItems.Add(newItem);
            entityMap.AddMessage($"You picked up {newItem.Name}.");
            entityMap.TagForRemoval(this);
        }
    }
}
