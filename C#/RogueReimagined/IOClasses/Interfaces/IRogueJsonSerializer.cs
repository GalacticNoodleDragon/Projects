using Rogue_Reimagined.Entities;

namespace Rogue_Reimagined.IOClasses.Interfaces
{
    public interface IRogueJsonSerializer
    {
        Task SerializeAsync(string saveName);
        List<Entity> Deserialize(string saveName);
    }
}
