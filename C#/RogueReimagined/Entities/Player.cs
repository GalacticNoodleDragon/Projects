using Rogue_Reimagined.IOClasses;
using Rogue_Reimagined.MiscClasses;
using Rogue_Reimagined.InGamePause;

namespace Rogue_Reimagined.Entities;

public class Player(char mapChar, Position position, int hp, int attack, int currentFloor) : Entity(mapChar, position)
{
    public int HealthPoints { get; set; } = hp;
    public int MaxHealtPoints { get; set; } = hp;
    public int Attack { get; set; } = attack;
    public int Level { get; set; } = 1;
    public int Experience { get; set; } = 0;
    public int NextLevelXP { get; set; } = Constants.BaseXP;
    public int CurrentFloor { get; set; } = currentFloor;
    public int Tick { get; set; } = Constants.TickSize;
    public Inventory Inventory { get; set; } = new();

    private void AttackEnemyIfNearby(in EntityMap entityMap)
    {
        foreach (Enemy enemy in entityMap.GetEnemies())
        {
            if (enemy.Position.IsInRange(Position))
            {
                entityMap.AddMessage($"Player hit Enemy for {Attack + Inventory.EquipedWeapon.Value} damage");
                enemy.HealthPoints -= Attack;
            }
        }
    }

    public override void Update(in EntityMap entityMap)
    {
        base.Update(entityMap);
        MadeAction = false;
        if (Console.KeyAvailable)
        {
            MadeAction = true;
            Position p = new(Position);
            var key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.W: --p.Y; break;
                case ConsoleKey.A: --p.X; break;
                case ConsoleKey.S: ++p.Y; break;
                case ConsoleKey.D: ++p.X; break;
                case ConsoleKey.C:
                {
                    RogueJsonSerializer serializer = new(entityMap.GetAll(), entityMap.GetPlayer());
                    Task.Run(() => serializer.SerializeAsync("save"));
                    entityMap.AddMessage("Saving game...");
                    break;
                }
                case ConsoleKey.P:
                {
                    PauseMenu();
                    break;
                }
            }

            Entity? at = entityMap.GetAtPos(p);
            if (at == null 
                || at.GetType() == typeof(Exit)
                || at.GetType() == typeof(Pickable))
            { 
                Position = p;
            }
        }

        if (LevelUp(entityMap))
            MadeAction = true;

        if (--Tick == 0)
        {
            MadeAction = true;
            AttackEnemyIfNearby(entityMap);
            Tick = Constants.TickSize;
        }
    }

    public bool LevelUp(in EntityMap entityMap)
    { 
        if (Experience >= NextLevelXP)
        {
            Experience -= NextLevelXP;
            NextLevelXP += Constants.BaseXP;
            MaxHealtPoints += Constants.BaseHP;
            HealthPoints = MaxHealtPoints;
            Attack += Constants.BaseAttack;
            ++Level;
            entityMap.AddMessage($"You are now level {Level}!");
            return true;
        }
        return false;
    }

    private void PauseMenu()
    {
        IPauseMenu pauseMenu = new PauseMenu();
        pauseMenu.RunMenu(this);
    }
}
