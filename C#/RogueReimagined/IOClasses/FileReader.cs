using Rogue_Reimagined.IOClasses.Interfaces;
using Rogue_Reimagined.Entities;

namespace Rogue_Reimagined.IOClasses;

public class FileReader : IFileReader
{
    public List<Entity> ReadLevelFile(string path, int level, Player player)
    {
        List<Entity> entities = [];

        using (StreamReader reader = new(path))
        {
            string? line = null;
            int count = 0;

            while ((line = reader.ReadLine()) != null)
            {
                for (int i = 0; i < line.Length; ++i)
                {
                    switch (line[i])
                    {
                        case '#': entities.Add(new Wall('#', new(i, count))); break;
                        case '!': entities.Add(new Pickable('!', new(i, count))); break;
                        case '%': entities.Add(new Exit('%', new(i, count))); break;
                        case 'E': entities.Add(new Enemy('E', new(i, count), Random.Shared.Next(5, 15) * level, Random.Shared.Next(3, 7) * level)); break;
                        case '@': entities.Add(player); player.Position = new(i, count); break;
                        default: break;
                    }
                }
                ++count;
            }
        }

        return entities;
    }
}