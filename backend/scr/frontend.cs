using Raylib_cs;

namespace BasicGameProject.Backend;

public class Frontend
{
    public int[][] tileMap;
    public List<Backend.Object> objects;
    public List<Backend.Player> players;
    public static void DrawObject(Backend.Object obj)
    {
        Raylib.DrawRectangle(obj.Position.X, obj.Position.Y, obj.CollisionSize.Width, obj.CollisionSize.Height, Color.BLUE);
    }

    public static void DrawPlayer(Backend.Player player)
    {
        Raylib.DrawRectangle(player.Position.X, player.Position.Y, player.CollisionSize.Width, player.CollisionSize.Height, Color.GREEN);
    }
}