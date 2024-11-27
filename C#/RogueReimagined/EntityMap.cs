using Rogue_Reimagined.Entities;
using Rogue_Reimagined.MiscClasses;

namespace Rogue_Reimagined;

public class EntityMap
{
    List<Entity> AllEntities { get; set; }
    List<Enemy> Enemies { get; set; }
    Player Player { get; set; }
    Exit Exit { get; set; }
    LinkedList<string> Messages { get; set; } = new();
    List<Entity> ToRemove { get; set; } = [];

    public EntityMap(List<Entity> entities)
    {
        AllEntities = entities;

        Entity? player = AllEntities.Where(e => e.GetType() == typeof(Player)).ToList().First();
        Player = (Player) player;

        Exit = (Exit) AllEntities.Where(e => e.MapChar == '%').First();

        // Should Have no Null members
        Enemies = AllEntities.Where(e => e.GetType() == typeof(Enemy)).Select(e => e as Enemy).ToList();
    }

    public Entity? GetAtPos(Position pos)
    {
        List<Entity> found = AllEntities.Where(e => e.Position.Equals(pos)).ToList();
        
        if (found.Count == 0)
            return null;

        return found[0];
    }

    public List<Entity> GetAll()
    {
        return AllEntities;
    }

    public Player GetPlayer()
    {
        return Player;
    }

    public List<Enemy> GetEnemies()
    {
        return Enemies;
    }

    public Exit GetExit()
    {
        return Exit;
    }

    public void AddMessage(string message)
    {
        Messages.AddLast(message);
        if (Messages.Count >= Constants.MaxMessageCount)
            Messages.RemoveFirst();
    }

    public LinkedListNode<string>? GetFirstMessage()
    {
        return Messages.First;
    }

    public void TagForRemoval(Entity entity)
    {
        ToRemove.Add(entity);
    }

    public void RemoveTagged()
    {
        while (ToRemove.Count > 0)
        {
            AllEntities.Remove(ToRemove.Last());
            ToRemove.RemoveAt(ToRemove.Count - 1);
        }
    }

    public bool ActionHasHappened()
    {
        foreach (Enemy enemy in Enemies)
            if (enemy.MadeAction)
                return true;

        if (Player.MadeAction)
            return true;

        return false;
    }
}
