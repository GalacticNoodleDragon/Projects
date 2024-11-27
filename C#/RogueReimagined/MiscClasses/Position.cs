namespace Rogue_Reimagined.MiscClasses;

public class Position
{
    public int X { get; set; }
    public int Y { get; set; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Position(Position other)
    {
        X = other.X;
        Y = other.Y;
    }

    public bool IsNearby(Position position)
    {
        return Math.Abs(position.X - X) <= 7 && Math.Abs(position.Y - Y) <= 7;
    }

    public bool IsInRange(Position position)
    {
        return Math.Abs(position.X - X) <= 1 && Math.Abs(position.Y - Y) <= 1;
    }

    public bool InVisibleRange(Position position)
    {
        return Math.Abs(position.X - X) <= 4 && Math.Abs(position.Y - Y) <= 4;
    }

    public override bool Equals(object? obj)
    {

        if (obj is not Position) 
            return false;

        Position? other = obj as Position;

        if (other == null)
            return false;

        return other.Y == Y && other.X == X;
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}