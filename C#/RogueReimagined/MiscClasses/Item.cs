namespace Rogue_Reimagined.MiscClasses;

public class Item(string name, string type, int value)
{
    public string Name { get; set; } = name;
    public string Type { get; set; } = type;
    public int Value { get; set; } = value;
}
