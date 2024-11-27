using Rogue_Reimagined.MiscClasses;

namespace Rogue_Reimagined.Entities;

public abstract class Entity(char mapChar, Position position)
{
    public char MapChar { get; set; } = mapChar;
    public Position Position { get; set; } = position;
    public bool MadeAction { get; set; } = false;
    public bool IsVisible { get; set; } = false;

    public char GetSymbol()
    {
        return MapChar;
    }
    public Position GetPosition()
    {
        return Position;
    }
    public virtual void Update(in EntityMap entityMap)
    {
        if (entityMap.GetPlayer().Position.InVisibleRange(Position))
            IsVisible = true;
    }
}