namespace Rogue_Reimagined.EntitiesDto;

public class EntityDto
{
    public string EntityType { get; set; } = "";
    public char MapChar { get; set; } = ' ';
    public int PositionX { get; set; } = -1;
    public int PositionY { get; set; } = -1;
    public int HealthPoints { get; set; } = -1;
    public int MaxHealtPoints { get; set; } = -1;
    public int Attack { get; set; } = -1;
    public int Experience { get; set; } = -1;
    public int NextLevelExperience { get; set; } = -1;
    public int Level { get; set; } = -1;
    public int CurrentFloor { get; set; } = -1;
    public bool IsVisible { get; set; } = false;
}
