namespace BasicGameProject.Backend;

using Raylib_cs;

public enum ObjectType
{
    Resource,
    Humanoid,
    Storage,
    CraftingStation,
    Player,
    Pickup,
    Core
}

public enum ResourceType
{
    Wood,
    Stone,
    Ore,
    Bar,
    Leather,
    Meat,
    Veggie
}

public struct Position
{
    public float X;
    public float Y;

    public Position(float x, float y)
    {
        X = x;
        Y = y;
    }
}

public struct PositionInt
{
    public int X;
    public int Y;

    public PositionInt(int x, int y)
    {
        X = x;
        Y = y;
    }
}

public struct Movement
{
    public float DeltaX;
    public float DeltaY;

    public Movement(float deltaX, float deltaY)
    {
        DeltaX = deltaX;
        DeltaY = deltaY;
    }
}

public struct Size 
{
    public int Width = 0;
    public int Height = 0;
    public Size(int width, int height)
    {
        Width = width;
        Height = height;
    }
}

public interface IDamagable
{
    int Health { get; set; }
    int MaxHealth { get; set; }
    ArmorValues Armor { get; set; }
}

public class Humanoid : Backend.Object, IDamagable
{
    public float MaxSpeed { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public ArmorValues Armor { get; set; }

    public void Push(Movement movement, List<Backend.Object> objects, bool includeCollision = true)
    {
        var newPosition = new Position(Position.X + movement.DeltaX, Position.Y + movement.DeltaY);
        if (includeCollision && IsCollision(objects, newPosition))
            return;

        Position = newPosition;
    }

    public Humanoid(ObjectType type, Position position, Size size, Color color, float maxSpeed, int health, ArmorValues armor) : base(type, position, size, color)
    {
        MaxSpeed = maxSpeed;
        Health = health;
        MaxHealth = health;
        Armor = armor;
    }
}

public interface IStructure : IDamagable
{

}

public interface IResource : IDamagable
{
    public ResourceType Type { get; set; }
}

public class ResourceContainer : Dictionary<ResourceType, int>
{
    
}