using Rogue_Reimagined.ConsoleWriter;
using Rogue_Reimagined.Entities;
using Rogue_Reimagined.MiscClasses;

namespace Rogue_Reimagined.InGamePause;

public class PauseMenu : IPauseMenu
{
    IGameConsoleWriter _conosleWriter = new GameConsoleWriter();

    public void RunMenu(Player player)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        _conosleWriter.PrintHelp();
        Console.ForegroundColor = ConsoleColor.White;
        string? line;
        do
        {
            Console.Write(">>> ");
            line = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            switch (line)
            {
                case "equip": TryEquip(player); break;
                case "inventory": _conosleWriter.PrintInventory(player); break;
                case "return": break;
                case "help": _conosleWriter.PrintHelp(); break;
                case "quit": Environment.Exit(0); break;
                default: Console.WriteLine("[UNKNOWN COMMAND]"); break;
            }
            Console.ForegroundColor = ConsoleColor.White;
        } while (line != "return");
        Console.Clear();
    }

    private void TryEquip(Player player)
    {
        Console.WriteLine("+-------------------------------------------");
        if (player.Inventory.CollectedItems.Count == 0)
        {
            Console.WriteLine("| No items in backpack to equip");
            Console.WriteLine("+-------------------------------------------");
            return;
        }

        Console.WriteLine("| Write a number of an item you want to equip: ");
        Console.Write("| ");
        string line = Console.ReadLine();
        bool retval = Int32.TryParse(line, out int a);
        if (!retval)
        {
            Console.WriteLine("| Invalid value");
            Console.WriteLine("+-------------------------------------------");
            return;
        }
        if (a < 0 || a >= player.Inventory.CollectedItems.Count)
        {
            Console.WriteLine("| Out of range");
            Console.WriteLine("+-------------------------------------------");
            return;
        }

        List<Item> backpack = player.Inventory.CollectedItems;
        if (backpack[a].Type == "armor")
        {
            (backpack[a], player.Inventory.EquipedArmor) = (player.Inventory.EquipedArmor, backpack[a]);
            player.MaxHealtPoints += player.Inventory.EquipedArmor.Value - backpack[a].Value;
            if (player.HealthPoints > player.MaxHealtPoints)
                player.HealthPoints = player.MaxHealtPoints;
            Console.WriteLine($"| You equipped {player.Inventory.EquipedArmor.Name}");
        }
        else
        {
            (backpack[a], player.Inventory.EquipedWeapon) = (player.Inventory.EquipedWeapon, backpack[a]);
            player.Attack += player.Inventory.EquipedWeapon.Value - backpack[a].Value;
            Console.WriteLine($"| You equipped {player.Inventory.EquipedWeapon.Name}");

        }

        Console.WriteLine("+-------------------------------------------");
    }
}