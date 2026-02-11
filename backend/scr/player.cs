namespace BasicGameProject.Backend;

using Raylib_cs;

public class Player : Humanoid
{
    public Player(Position position) : base(ObjectType.Player, position, new Size(32, 32), new Color(0, 0, 255, 255), 5.0f, 100, new ArmorValues(1, 1, 1))
    {

    }
}