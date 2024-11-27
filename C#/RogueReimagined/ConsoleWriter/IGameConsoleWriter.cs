using Rogue_Reimagined.Entities;

namespace Rogue_Reimagined.ConsoleWriter;

public interface IGameConsoleWriter
{
    public void PrintGame(EntityMap map);
    public void PrintMenu(int inSelection);
    public void PrintEndWon();
    public void PrintEndLost();
    public void PrintHelp();
    public void PrintInventory(Player player);
    public void WriteBlank();
}
