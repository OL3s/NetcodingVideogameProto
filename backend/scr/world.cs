namespace BasicGameProject.Backend;

public class World
{
    public List<Player> Players = new List<Player>();
    public List<Object> Objects = new List<Object>();
    public Size Size { get; set; }
    public int[,] TileMap { get; set; }
    public int TileSize { get; set; } = 16;
    public World(Size size)
    {
        Size = size;
        TileMap = new int[size.Height, size.Width];
    }
    public void GenerateWorld(WorldGenerationParameters parameters)
    {
        WorldGenerationTools.GenerateRandomWorld(this, parameters);
    }

}



