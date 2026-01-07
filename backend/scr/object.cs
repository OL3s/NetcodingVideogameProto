using System.Drawing;

namespace BasicGameProject.Backend;

public abstract class Object
{
    public ObjectType Type;
    public Position Position;
    public Size CollisionSize;

    public Object(ObjectType type, Position position, CollisionSize collisionSize)
    {
        Type = type;
        Position = position;
        CollisionSize = collisionSize;
    }
    public void Teleport(Position position)
    {
        Position = position;
    }
    public bool IsCollision(List<Object> objects, Position position = null)
    {
        int thisWidth = CollisionSize.Width;
        int thisHeight = CollisionSize.Height;
        position ??= Position;

        foreach (var o in objects)
        {
            if (o == this) continue;

            int otherWidth = o.CollisionSize.Width;
            int otherHeight = o.CollisionSize.Height;

            return  position.X < o.Position.X + otherWidth &&
                    position.X + thisWidth > o.Position.X &&
                    position.Y < o.Position.Y + otherHeight &&
                    position.Y + thisHeight > o.Position.Y;
        }
        return false;
    }

    public bool IsCollision(List<Object> objects)
    {
        return IsCollision(objects, Position);
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

            return  position.X < o.Position.X + otherWidth &&
                    position.X + thisWidth > o.Position.X &&
                    position.Y < o.Position.Y + otherHeight &&
                    position.Y + thisHeight > o.Position.Y;
        }
        return false;
    }
}