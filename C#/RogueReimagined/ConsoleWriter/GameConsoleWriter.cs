using Rogue_Reimagined.Entities;
using Rogue_Reimagined.MiscClasses;

namespace Rogue_Reimagined.ConsoleWriter;

public class GameConsoleWriter : IGameConsoleWriter
{
    public GameConsoleWriter()
    {
        Console.Clear();
        Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
    }

    public void PrintMenu(int inSelection)
    {
        Console.SetCursorPosition(0, 0);
        Console.WriteLine("  _____                        ");
        Console.WriteLine(" |  __ \\                       ");
        Console.WriteLine(" | |__) |___   __ _ _   _  ___ ");
        Console.WriteLine(" |  _  // _ \\ / _` | | | |/ _ \\");
        Console.WriteLine(" | | \\ \\ (_) | (_| | |_| |  __/");
        Console.WriteLine(" |_|  \\_\\___/ \\__, |\\__,_|\\___|");
        Console.WriteLine("               __/ |           ");
        Console.WriteLine("              |___/ ");
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("         " + (inSelection == 0 ? "> " : "  ") + "New Game");
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("         " + (inSelection == 1 ? "> " : "  ") + "Load Game");
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("         " + (inSelection == 2 ? "> " : "  ") + "Quit");
    }

    public void PrintGame(EntityMap entityMap)
    {
        Console.SetCursorPosition(0, 0);
        Console.CursorVisible = false;
        LinkedListNode<string>? messageNode = entityMap.GetFirstMessage();

        Console.WriteLine("----------------------------------------------------------------------------------+-----------------");
        for (int y = 0; y <= 30; ++y)
        {
            Console.Write("|");
            for (int x = 0; x <= 80; ++x)
            {
                Entity? atPos = entityMap.GetAtPos(new(x, y));
                    PrintEntityChar(atPos == null || !atPos.IsVisible ? ' ' : atPos.MapChar);
            }

            if (y == 0)
            {
                Console.WriteLine($"| HP: {entityMap.GetPlayer().HealthPoints}/{entityMap.GetPlayer().MaxHealtPoints}".PadRight(20));
            }
            else if (y == 1)
            {
                Console.WriteLine($"| Attack: {entityMap.GetPlayer().Attack}".PadRight(20));
            }   
            else if (y == 2)
            {
                Console.WriteLine($"| XP: {entityMap.GetPlayer().Experience}/{entityMap.GetPlayer().NextLevelXP}".PadRight(20));
            }
            else if (y == 3)
            {
                Console.WriteLine($"| Level: {entityMap.GetPlayer().Level}".PadRight(20));
            }
            else if (y == 4)
            {
                Console.WriteLine("+-------------------------");
            }
            else if (messageNode != null)
            {
                Console.WriteLine($"| {messageNode.Value}".PadRight(50));
                messageNode = messageNode.Next;
            }
            else
            {
                Console.WriteLine("|");
            }
        }
        Console.WriteLine("----------------------------------------------------------------------------------+-----------------");
    }

    private void PrintEntityChar(char c)
    {
        switch (c)
        {
            case '@': Console.ForegroundColor = ConsoleColor.Green; break;
            case 'E': Console.ForegroundColor = ConsoleColor.Red; break;
            case '!': Console.ForegroundColor = ConsoleColor.Yellow; break;
            case '%': Console.ForegroundColor = ConsoleColor.Cyan; break;
            default: break;
        }
        Console.Write(c);
        Console.ForegroundColor = ConsoleColor.White;
    }

    public void PrintEndWon()
    {
        Console.Clear();
        Console.SetCursorPosition(0, 10);
        Console.WriteLine("                 __     __          __          __         ");
        Console.WriteLine("                 \\ \\   / /          \\ \\        / /         ");
        Console.WriteLine("                  \\ \\_/ /__  _   _   \\ \\  /\\  / /__  _ __  ");
        Console.WriteLine("                   \\   / _ \\| | | |   \\ \\/  \\/ / _ \\| '_ \\ ");
        Console.WriteLine("                    | | (_) | |_| |    \\  /\\  / (_) | | | |");
        Console.WriteLine("                    |_|\\___/ \\__,_|     \\/  \\/ \\___/|_| |_|");
    }

    public void PrintEndLost()
    {
        Console.Clear();
        Console.SetCursorPosition(0, 10);
        Console.WriteLine("                __     __           _               _   ");
        Console.WriteLine("                \\ \\   / /          | |             | |  ");
        Console.WriteLine("                 \\ \\_/ /__  _   _  | |     ___  ___| |_ ");
        Console.WriteLine("                  \\   / _ \\| | | | | |    / _ \\/ __| __|");
        Console.WriteLine("                   | | (_) | |_| | | |___| (_) \\__ \\ |_ ");
        Console.WriteLine("                   |_|\\___/ \\__,_| |______\\___/|___/\\__|");

    }

    public void WriteBlank()
    {
        Console.SetCursorPosition(0, 0);
        for (int x = 0;x <= 35; ++x)
            Console.WriteLine(new string(' ', 150));
    }

    public void PrintHelp()
    {
        Console.WriteLine("+-------------------------------------------");
        Console.WriteLine("| Character controls: ");
        Console.WriteLine("| W - Move up");
        Console.WriteLine("| A - Move left");
        Console.WriteLine("| S - Move down");
        Console.WriteLine("| D - Move right");
        Console.WriteLine("| C - Save game");
        Console.WriteLine("| P - Start pause menu");
        Console.WriteLine("+-------------------------------------------");
        Console.WriteLine("| Pause menu commands: ");
        Console.WriteLine("| equip - equip an item from your inventory");
        Console.WriteLine("| inventory - show your inventory");
        Console.WriteLine("| return - resume your game");
        Console.WriteLine("| help - print out help");
        Console.WriteLine("| quit - close the game");
        Console.WriteLine("+-------------------------------------------");
        Console.WriteLine("| Entities: ");
        Console.WriteLine("| @ - player");
        Console.WriteLine("| # - wall");
        Console.WriteLine("| ! - item");
        Console.WriteLine("| E - enemy");
        Console.WriteLine("| % - exit");
        Console.WriteLine("+-------------------------------------------");
    }

    public void PrintInventory(Player player)
    {
        Console.WriteLine("+---------------------------------------------------------");
        Console.WriteLine("| Items in backpack: ");
        int i = 0;
        foreach (Item item in player.Inventory.CollectedItems)
        {
            PrintItem(item, i.ToString());
            ++i;
        }
        Console.WriteLine("+---------------------------------------------------------");
        Console.WriteLine("| Equipped items: ");
        PrintItem(player.Inventory.EquipedWeapon, " ");
        PrintItem(player.Inventory.EquipedArmor, " ");
        Console.WriteLine("+---------------------------------------------------------");
    }

    private void PrintItem(Item item, string id)
    {
         Console.WriteLine(
            "| " +
            id.PadLeft(3, ' ') +
            " | " +
            item.Name.PadRight(25, ' ') +
            " | " +
            item.Type.PadRight(6, ' ') +
            " | " +
            (item.Type == "armor" ? "HP: " : "Attack: ") +
            item.Value
        );
    }
}
