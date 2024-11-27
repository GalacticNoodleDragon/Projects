using Rogue_Reimagined.Entities;

namespace Rogue_Reimagined.IOClasses.Interfaces;

public interface IFileReader
{
    List<Entity> ReadLevelFile(string path, int level, Player player);
}
