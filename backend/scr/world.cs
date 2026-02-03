namespace BasicGameProject.Backend;

public class World
{
    public List<Player> Players = new List<Player>();
    public List<Backend.Object> Objects = new List<Backend.Object>();
    public Size Size { get; set; }
    public int[,] TileMap { get; set; }
    public int TileSize { get; set; } = 16;
    public World(Size size)
    {
        Size = size;
        TileMap = new int[size.Height, size.Width];
    }
    public void GenerateWorld()
    {
        for (int y = 0; y < Size.Height; y++)
        {
            for (int x = 0; x < Size.Width; x++)
            {
                TileMap[y, x] = Random.Shared.Next(0, 2); // Default tile
            }
        }
    }
}

