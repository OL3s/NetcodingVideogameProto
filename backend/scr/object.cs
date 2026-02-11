namespace BasicGameProject.Backend;

using Raylib_cs;

public abstract class Object
{
    public int Id;
    public ObjectType Type;
    public Position Position;
    public Size CollisionSize;
    public Color Color;

    public Object(ObjectType type, Position position, Size collisionSize, Color color, int id = -1)
    {
        Type = type;
        Position = position;
        CollisionSize = collisionSize;
        Color = color;
        Id = id;
    }
    public void Teleport(Position position)
    {
        Position = position;
    }
    public bool IsCollision(List<Object> objects)
    {
        return IsCollision(objects, Position);
    }

    public bool IsCollision(List<Object> objects, Position position)
    {
        int thisWidth = CollisionSize.Width;
        int thisHeight = CollisionSize.Height;

        foreach (var o in objects)
        {
            if (o == this) continue;

            int otherWidth = o.CollisionSize.Width;
            int otherHeight = o.CollisionSize.Height;

            bool overlap = position.X < o.Position.X + otherWidth &&
                           position.X + thisWidth > o.Position.X &&
                           position.Y < o.Position.Y + otherHeight &&
                           position.Y + thisHeight > o.Position.Y;

            if (overlap)
                return true;
        }
        return false;
    }

    public bool IsCollision(List<Object> objects, Movement movement)
    {
        return IsCollision(objects, new Position(Position.X + movement.DeltaX, Position.Y + movement.DeltaY));
    }

    public static bool IsCollisionAtPosition(List<Object> objects, Position position, Size collisionSize)
    {
        int thisWidth = collisionSize.Width;
        int thisHeight = collisionSize.Height;

        foreach (var o in objects)
        {
            int otherWidth = o.CollisionSize.Width;
            int otherHeight = o.CollisionSize.Height;

            bool overlap = position.X < o.Position.X + otherWidth &&
                           position.X + thisWidth > o.Position.X &&
                           position.Y < o.Position.Y + otherHeight &&
                           position.Y + thisHeight > o.Position.Y;

            if (overlap)
                return true;
        }
        return false;
    }
}

public class Tree : Object
{
    public Tree(Position position, int id) : base(ObjectType.Resource, position, new Size(16, 16), new Color(34, 139, 34, 255), id)
    {
    }
}

public class Rock : Object
{
    public Rock(Position position, int id) : base(ObjectType.Resource, position, new Size(16, 16), new Color(100, 100, 100, 255), id)
    {
    }
}