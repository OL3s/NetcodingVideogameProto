namespace BasicGameProject.Backend;
public static class WorldGenerationTools
{
    public static void GenerateRandomWorld(World world, WorldGenerationParameters parameters)
    {
        int[,] mapping = new int[world.Size.Height, world.Size.Width];
        GenerateNoice(mapping);
        MappingSmooth(mapping, radius: parameters.radius, iterations: parameters.iterations);
        GenerateObjects(world, mapping);
        world.TileMap = mapping;
    }
    private static void GenerateNoice(int[,] mapping)
    {
        for (int y = 0; y < mapping.GetLength(0); y++)
        {
            for (int x = 0; x < mapping.GetLength(1); x++)
            {
                mapping[y, x] = Random.Shared.Next(0, 2) == 0 ? 2 : 0;
            }
        }
    }
    private static void GenerateObjects(World world, int[,] mapping)
    {
        if (mapping == null) return;
        if (world.Objects.Count > 0) return; // only generate if there are no objects, to avoid duplicates

        var (treePositions, rockPositions) = GetMapping(mapping);
        int idCounter = 0;
        foreach (var (x, y) in treePositions)
        {
            if (Random.Shared.NextDouble() < 0.4)
            {
                var offsetX = Random.Shared.Next(0, 5) - 2;
                var offsetY = Random.Shared.Next(0, 5) - 2;
                world.Objects.Add(
                    new Tree(new Position(
                        x * world.TileSize + (world.TileSize / 2) + offsetX, 
                        y * world.TileSize + (world.TileSize / 2) + offsetY
                    ), 
                    idCounter++));
            }
        }
        foreach (var (x, y) in rockPositions)
        {
            if (Random.Shared.NextDouble() < 0.3) 
            {
                var offsetX = Random.Shared.Next(0, 5) - 2;
                var offsetY = Random.Shared.Next(0, 5) - 2;
                world.Objects.Add(
                    new Rock(new Position(
                        x * world.TileSize + (world.TileSize / 2) + offsetX, 
                        y * world.TileSize + (world.TileSize / 2) + offsetY
                    ), 
                    idCounter++));
            }
        }
    }
    private static void MappingSmooth(int[,] mapping, int radius, int iterations)
    {
        int width = mapping.GetLength(1);
        int height = mapping.GetLength(0);
        for (int i = 0; i < iterations; i++)
        {
            float[,] newMapping = new float[height, width];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int sum = 0;
                    int count = 0;
                    for (int dy = -radius; dy <= radius; dy++)
                    {
                        for (int dx = -radius; dx <= radius; dx++)
                        {
                            int nx = x + dx;
                            int ny = y + dy;
                            if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                            {
                                sum += mapping[ny, nx];
                                count++;
                            }
                        }
                    }
                    newMapping[y, x] = (float)sum / count;
                }
            }
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    mapping[y, x] = (int)Math.Round(newMapping[y, x]);
                }
            }
        }
    }

    private static (HashSet<(int, int)> tree, HashSet<(int, int)> rock) GetMapping(int[,] tileMap)
    {
        HashSet<(int, int)> treePositions = new HashSet<(int, int)>();
        HashSet<(int, int)> rockPositions = new HashSet<(int, int)>();
        for (int y = 0; y < tileMap.GetLength(0); y++)
        {
            for (int x = 0; x < tileMap.GetLength(1); x++)
            {
                switch (tileMap[y, x])
                {
                    case 0:
                        treePositions.Add((x, y));
                        break;
                    case 2:
                        rockPositions.Add((x, y));
                        break;
                }
            }
        }
        return (treePositions, rockPositions);
    }
}

public struct WorldGenerationParameters
{
    public int radius { get; set; }
    public int iterations { get; set; }
    public WorldGenerationParameters(int radius, int iterations)
    {        
        this.radius = radius;
        this.iterations = iterations;
    }
}