using Rogue_Reimagined.ConsoleWriter;
using Rogue_Reimagined.Entities;
using Rogue_Reimagined.Game;
using Rogue_Reimagined.IOClasses;

namespace Rogue_Reimagined.Runner;

public class GameRunner : IGameRunner
{
    private readonly IGameConsoleWriter _consoleWriter;
    private string _mainDir = $"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName}";

    public GameRunner()
    {
        _consoleWriter = new GameConsoleWriter();
    }

    public void Run()
    {
        int retval = RunMenu();
        string path = retval switch
        {
            0 => _mainDir + "\\Levels\\Level",
            1 => "save",
            2 => "",
            _ => "",
        };

        if (path == "save" && !File.Exists(_mainDir + "\\Saved\\save.json"))
            path = _mainDir + "\\Levels\\Level";

        Player player;

        if (path == "")
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("You Quit");
            return;
        }
        else if (path == "save")
        {
            RogueJsonSerializer serializer = new ();
            List<Entity> entities = serializer.Deserialize(path);
            player = (Player) entities.Where(e => e.GetType() == typeof(Player)).First();
            LevelRunner level = new(entities, _consoleWriter);
            level.RunLevel();
            path = _mainDir + "\\Levels\\Level";
        }
        else
        {
            player = new('@', new(0, 0), MiscClasses.Constants.BaseHP, MiscClasses.Constants.BaseAttack, 1);
        }


        while (player.CurrentFloor <= MiscClasses.Constants.LevelCount)
        {
            LevelRunner level = new(path + player.CurrentFloor.ToString() + ".txt", _consoleWriter, player);
            level.RunLevel();
        }

        _consoleWriter.PrintEndWon();
        Console.ReadKey(true);
    }

    private int RunMenu()
    {
        bool enterPressed = false;
        int status = 0;

        while (!enterPressed)
        {
            _consoleWriter.PrintMenu(status);

            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.W)
            {
                if (status == 0)
                {
                    status = 2;
                }
                else
                {
                    status = (status - 1) % 3;
                }
            }
            else if (key.Key == ConsoleKey.S)
            {
                status = (status + 1) % 3;
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                enterPressed = true;
            }
        }

        return status;
    }
}
