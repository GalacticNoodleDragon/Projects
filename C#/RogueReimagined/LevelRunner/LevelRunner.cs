using Rogue_Reimagined.ConsoleWriter;
using Rogue_Reimagined.Entities;
using Rogue_Reimagined.IOClasses;

namespace Rogue_Reimagined.Game;

public class LevelRunner : ILevelRunner
{
    public EntityMap EntityMap { get; set; }

    private IGameConsoleWriter _consoleWriter;
    private bool _foundExit = false;

    public LevelRunner(string path, IGameConsoleWriter gameConsoleWriter, Player player)
    {
        FileReader fileReader = new();
        EntityMap = new(fileReader.ReadLevelFile(path, player.CurrentFloor, player));

        _consoleWriter = gameConsoleWriter;
        _consoleWriter.WriteBlank();
    }

    public LevelRunner(List<Entity> entities, IGameConsoleWriter gameConsoleWriter)
    {
        EntityMap = new(entities);

        _consoleWriter = gameConsoleWriter;
        _consoleWriter.WriteBlank();
    }

    public void RunLevel()
    {
        UpdateLevel();
        _consoleWriter.PrintGame(EntityMap);
        while (!_foundExit)
        {
            UpdateLevel();
            if (EntityMap.GetPlayer().HealthPoints <= 0)
            {
                _consoleWriter.PrintEndLost();
                Console.ReadKey(true);
                Environment.Exit(0);
            }

            if (EntityMap.ActionHasHappened())
                 _consoleWriter.PrintGame(EntityMap);
        }

        ++EntityMap.GetPlayer().CurrentFloor;
    }

    public void UpdateLevel()
    {
        if (EntityMap.GetPlayer().Position.Equals(EntityMap.GetExit().Position))
            _foundExit = true;

        foreach (Entity entity in EntityMap.GetAll())
            entity.Update(EntityMap);
        
        EntityMap.RemoveTagged();
    }
}
