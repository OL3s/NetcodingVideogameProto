namespace BasicGameProject.Backend;

public class Player : Humanoid
{
    public Player(Position position) : base(ObjectType.Player, position, new Size(32, 32), 5.0f, 100, new ArmorValues(1, 1, 1))
    {

    }
}