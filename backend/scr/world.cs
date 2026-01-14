namespace BasicGameProject.Backend;

public class World
{
    public List<Player> Players = new List<Player>();
    public List<Backend.Object> Objects = new List<Backend.Object>();
    public Size Size { get; set; }
    public int[,]? TileMap { get; set; }
    public void PrintWorldTiles()
    {
        if (TileMap is null)
            return;

        for (int y = 0; y < TileMap.GetLength(0); y++)
        {
            for (int x = 0; x < TileMap.GetLength(1); x++)
            {
                char tileChar = TileMap[y, x] switch
                {
                    -1 => '0',
                    0 => '.',
                    1 => 'X',
                    _ => ' ',
                };

                Console.Write(tileChar);
            }

            Console.WriteLine();
        }
    }
}


public class WorldGenerator
{
    public Size SizeCompact { get; set; }
    public int TileSize { get; set; }
    public Size RealSize => new Size(SizeCompact.Width * TileSize, SizeCompact.Height * TileSize);
    public List<Backend.Object> Objects { get; set; }
    public List<Backend.Player> Players { get; set; }
    public WorldGenerator()
    {
        Objects = new List<Backend.Object>();
        Players = new List<Backend.Player>();
    }
    public void SetValues(Size sizeCompact, int tileSize)
    {
        SizeCompact = sizeCompact;
        TileSize = tileSize;
    }
    private int[,] SectionedNoiceMap()
    {
        int[,] generateMap = new int[SizeCompact.Height, SizeCompact.Width];

        // Make raw noicemap
        for(int y = 0; y < SizeCompact.Height; y++)
        {
            for(int x = 0; x < SizeCompact.Width; x++)
            {
                generateMap[y, x] = (new Random().NextDouble() < .5) ? -1 : 1;
            }
        }
        return generateMap;
    }
    private float GetAvgRadius(int radius, PositionInt position, int[,] map)
    {
        int total = 0;
        int count = 0;
        for(int y = -radius; y <= radius; y++)
        {
            for(int x = -radius; x <= radius; x++)
            {
                int checkX = position.X + x;
                int checkY = position.Y + y;
                if(checkX >= 0 && checkX < map.GetLength(1) && checkY >= 0 && checkY < map.GetLength(0))
                {
                    total += map[checkY, checkX];
                    count++;
                }
            }
        }
        return (count == 0) ? 0f : (float)(total / count);
    }

    private float GetAvgRadius(int radius, PositionInt position, float[,] map)
    {
        float total = 0;
        int count = 0;
        for(int y = -radius; y <= radius; y++)
        {
            for(int x = -radius; x <= radius; x++)
            {
                int checkX = position.X + x;
                int checkY = position.Y + y;
                if(checkX >= 0 && checkX < map.GetLength(1) && checkY >= 0 && checkY < map.GetLength(0))
                {
                    total += map[checkY, checkX];
                    count++;
                }
            }
        }
        return (count == 0) ? 0f : (float)(total / count);
    }


    /// SmoothMap a int map -> float map
    private float[,] SmoothMap(int[,] map, int smoothRadius)
    {
        float[,] returnMap = new float[map.GetLength(0), map.GetLength(1)];
        for(int y = 0; y < map.GetLength(0); y++)
        {
            for(int x = 0; x < map.GetLength(1); x++)
            {
                returnMap[y, x] = GetAvgRadius(smoothRadius, new PositionInt(x, y), map);
            }
        }
        return returnMap;
    }

    private float[,] SmoothMap(float[,] map, int smoothRadius)
    {
        float[,] returnMap = new float[map.GetLength(0), map.GetLength(1)];
        for(int y = 0; y < map.GetLength(0); y++)
        {
            for(int x = 0; x < map.GetLength(1); x++)
            {
                returnMap[y, x] = GetAvgRadius(smoothRadius, new PositionInt(x, y), map);
            }
        }
        return returnMap;
    }

    /// SeparateRegions from a float map -> int map {-1, 1}
    private int[,] SeperateRegions(float[,] map)
    {
        int[,] returnMap = new int[map.GetLength(0), map.GetLength(1)];
        for(int y = 0; y < map.GetLength(0); y++)
        {
            for(int x = 0; x < map.GetLength(1); x++)
            {
                if(map[y, x] < 0)
                {
                    returnMap[y, x] = -1;
                }
                else
                {
                    returnMap[y, x] = 1;
                }
            }
        }
        return returnMap;
    }

    /// RegionPadding from an int map {-1, 1} -> int map {-1, 0, 1}
    private int[,] RegionPadding(int[,] map, int padding)
    {
        int[,] returnMap = new int[map.GetLength(0), map.GetLength(1)];
        for(int y = 0; y < map.GetLength(0); y++)
        {
            for(int x = 0; x < map.GetLength(1); x++)
            {
                if(map[y, x] < 0)
                {
                    bool nearBorder = false;
                    for(int padY = -padding; padY <= padding; padY++)
                    {
                        for(int padX = -padding; padX <= padding; padX++)
                        {
                            int checkX = x + padX;
                            int checkY = y + padY;
                            if(checkX >= 0 && checkX < map.GetLength(1) && checkY >= 0 && checkY < map.GetLength(0))
                            {
                                if(map[checkY, checkX] > 0)
                                {
                                    nearBorder = true;
                                }
                            }
                        }
                    }
                    if(nearBorder)
                    {
                        returnMap[y, x] = 0;
                    }
                    else
                    {
                        returnMap[y, x] = -1;
                    }
                }
                else
                {
                    returnMap[y, x] = 1;
                }
            }
        }
        return returnMap;
    }

    private List<Backend.Object> ConvertMapToObjects(int[,] map)
    {
        List<Backend.Object> objects = new List<Backend.Object>();
        for(int y = 0; y < map.GetLength(0); y++)
        {
            for(int x = 0; x < map.GetLength(1); x++)
            {
                if (new Random().NextDouble() < .9) continue;
                Backend.Object? objectCreate = map[y, x] switch
                {
                    -1 => new Rock(new Position(x, y), objects.Count),
                    1 => new Tree(new Position(x, y), objects.Count),
                    0 => null,
                    _ => null
                };
                if(objectCreate != null)
                {
                    objects.Add(objectCreate);
                }
            }
        }
        return objects;
    }

    private int[,] GenerateBiomeTilemap() 
    {
        int[,] returnMap = new int[SizeCompact.Height, SizeCompact.Width];
        int center = SizeCompact.Height / 2;
        for(int y = 0; y < SizeCompact.Height; y++)
        {
            for(int x = 0; x < SizeCompact.Width; x++)
            {
                float distanceToCenter = y - center;
            }
        }
        throw new NotImplementedException();
    }

    public World GenerateWorld(int smoothRadius = 4, int regionPadding = 2, int repeatSmoothFunctions = 2)
    {
        int[,] noiseMap = SectionedNoiceMap();
        float[,] smoothMap = SmoothMap(noiseMap, smoothRadius);

        for(int i = 0; i < repeatSmoothFunctions; i++)
        {
            smoothMap = SmoothMap(smoothMap, smoothRadius);
        }

        int[,] regionedMap = SeperateRegions(smoothMap);
        int[,] paddedMap = RegionPadding(regionedMap, regionPadding);
        List<Backend.Object> objects = ConvertMapToObjects(paddedMap);

        World world = new World()
        {
            Size = RealSize,
            Objects = objects,
            Players = new List<Player>()
        };
        return world;
    }
}