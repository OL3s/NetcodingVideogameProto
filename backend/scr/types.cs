namespace BasicGameProject.Backend;

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
    public int Width;
    public int Height;
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
    ArmorValues armor { get; set; }
}

public interface IHumanoid : IDamagable
{
    float MaxSpeed { get; set; }

    public void Push(Movement movement, List<Object> objects, bool includeCollision = true)
    {
        if (includeCollision && IsCollision(objects, new Position(Position.X + movement.DeltaX, Position.Y + movement.DeltaY)))
            return;
        Position.X += movement.DeltaX;
        Position.Y += movement.DeltaY;
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