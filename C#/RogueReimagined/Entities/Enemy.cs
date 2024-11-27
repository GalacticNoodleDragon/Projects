using Rogue_Reimagined.MiscClasses;

namespace Rogue_Reimagined.Entities;

public class Enemy(char mapChar, Position position, int hp, int attack) : Entity(mapChar, position)
{
    public int HealthPoints { get; set; } = hp;
    public int MaxHealthPoints { get; set; } = hp;
    public int Attack { get; set; } = attack;
    public int Tick { get; set; } = Constants.TickSize;

    public override void Update(in EntityMap entityMap)
    {
        base.Update(entityMap);
        MadeAction = false;
        if (HealthPoints < 0)
        {
            MadeAction = true;
            entityMap.AddMessage("Enemy has been defeated.");
            entityMap.AddMessage($"You gained {MaxHealthPoints} XP");
            entityMap.GetPlayer().Experience += MaxHealthPoints;
            entityMap.TagForRemoval(this);
            entityMap.GetEnemies().Remove(this);
            return;
        }

        if (Tick-- > 0)
            return;


        if (Position.IsInRange(entityMap.GetPlayer().Position))
        {
            MadeAction = true;
            AttackPlayer(entityMap);
        }
        else if (Position.IsNearby(entityMap.GetPlayer().Position))
        {
            MadeAction = true;
            MoveTowards(in entityMap, entityMap.GetPlayer().Position);
        }

        Tick = Constants.TickSize;
    }

    private void MoveTowards(in EntityMap entityMap, Position pos)
    {
        Position newPos = new(Position);

        int moveX = pos.X > Position.X ? 1 : pos.X == Position.X ? 0 : -1;
        int moveY = pos.Y > Position.Y ? 1 : pos.Y == Position.Y ? 0 : -1;

        newPos.X += moveX;

        if (entityMap.GetAtPos(newPos) == null)
        {
            newPos.Y += moveY;
            if (entityMap.GetAtPos(newPos) == null)
            {
                Position = newPos;
                return;
            }
            newPos.Y -= moveY;
            Position = newPos;
            return;
        }

        newPos.X -= moveX;
        newPos.Y += moveY;
        if (entityMap.GetAtPos(newPos) == null)
        {
            Position = newPos;
            return;
        }
    }

    private void AttackPlayer(in EntityMap entityMap)
    {
        entityMap.AddMessage($"Enemy hit player for {Attack} damage");
        entityMap.GetPlayer().HealthPoints -= Attack;
    }
}