using System.Drawing;

namespace BasicGameProject.Backend;

public class Player : Object, IHumanoid
{
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public float MaxSpeed { get; set; }

    public Player(Position position) : base(ObjectType.Player, position, new Size(32, 32))
    {
        Health = 100;
        MaxHealth = 100;
        MaxSpeed = 5.0f;
    }
}