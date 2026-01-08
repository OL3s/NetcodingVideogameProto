using System.Collections.Generic;
using System.Dynamic;
using System.Security.Cryptography;

namespace BasicGameProject.Backend;

public class World
{
    public List<Player> Players = new List<Player>();
    public List<Object> Objects = new List<Object>();
    public Size Size { get; set; }
    public void PrintWorldTiles()
    {
        for(int y = 0; y < Size.Height; y++)
        {
            foreach(int x in Size.Width)
            {
                char tileChar = worldMap[y][x] switch
                {
                    -1 => '0',
                    0 => '.',
                    1 => 'X',
                    _ => ' ',
                };
                Console.Write(
                );
            }
        }
    }
}

public static class WorldGenerator
{
    public Size SizeCompact { get; set; }
    public int TileSize { get; set; }
    public Size RealSize => new Size(SizeCompact.X * TileSize, SizeCompact.Y * TileSize);
    public List<Object> Objects { get; set; }
    public List<Player> Players { get; set; }
    public void SetValues(Size sizeCompact, int tileSize)
    {
        SizeCompact = sizeCompact;
        TileSize = tileSize;
    }
    private int[][] SectionedNoiceMap()
    {
        int[SizeCompact.Height][SizeCompact.Width] generateMap = {};

        // Make raw noicemap
        for(int y = 0; y < SizeCompact.Height; y++)
        {
            for(int x = 0; x < SizeCompact.Width; x++)
            {
                returnMap[y][x] = (Random.NextDouble() < .5) ? -1 : 1;
            }
        }
    }
    private float GetAvgRadius(int radius, Position position, int[][] map)
    {
        int total = 0;
        int count = 0;
        for(int y = -radius; y <= radius; y++)
        {
            for(int x = -radius; x <= radius; x++)
            {
                int checkX = position.X + x;
                int checkY = position.Y + y;
                if(checkX >= 0 && checkX < map[0].Length && checkY >= 0 && checkY < map.Length)
                {
                    total += map[checkY][checkX];
                    count++;
                }
            }
        }
        return (count == 0) ? 0f : (float)total / count;
    }

    /// SmoothMap a int map -> float map
    private float[][] SmoothMap(int[][] map, int smoothRadius)
    {
        float[][] returnMap = new float[map.Length][map[0].Length];
        for(int y = 0; y < map.Length; y++)
        {
            for(int x = 0; x < map[0].Length; x++)
            {
                returnMap[y][x] = GetAvgRadius(smoothRadius, new Position(x, y), map);
            }
        }
        return returnMap;
    }

    private float[][] SmoothMap(float[][] map, int smoothRadius)
    {
        float[][] returnMap = new float[map.Length][map[0].Length];
        for(int y = 0; y < map.Length; y++)
        {
            for(int x = 0; x < map[0].Length; x++)
            {
                returnMap[y][x] = GetAvgRadius(smoothRadius, new Position(x, y), map);
            }
        }
        return returnMap;
    }

    /// SeparateRegions from a float map -> int map {-1, 1}
    private int[][] SeperateRegions(float[][] map)
    {
        int[][] returnMap = new int[map.Length][map[0].Length];
        for(int y = 0; y < map.Length; y++)
        {
            for(int x = 0; x < map[0].Length; x++)
            {
                if(map[y][x] < 0)
                {
                    returnMap[y][x] = -1;
                }
                else
                {
                    returnMap[y][x] = 1;
                }
            }
        }
        return returnMap;
    }

    /// RegionPadding from an int map {-1, 1} -> int map {-1, 0, 1}
    private int[][] RegionPadding(int[][] map, int padding)
    {
        int[][] returnMap = new int[map.Length][map[0].Length];
        for(int y = 0; y < map.Length; y++)
        {
            for(int x = 0; x < map[0].Length; x++)
            {
                if(map[y][x] < 0)
                {
                    bool nearBorder = false;
                    for(int padY = -padding; padY <= padding; padY++)
                    {
                        for(int padX = -padding; padX <= padding; padX++)
                        {
                            int checkX = x + padX;
                            int checkY = y + padY;
                            if(checkX >= 0 && checkX < map[0].Length && checkY >= 0 && checkY < map.Length)
                            {
                                if(map[checkY][checkX] > 0)
                                {
                                    nearBorder = true;
                                }
                            }
                        }
                    }
                    if(nearBorder)
                    {
                        returnMap[y][x] = 0;
                    }
                    else
                    {
                        returnMap[y][x] = -1;
                    }
                }
                else
                {
                    returnMap[y][x] = 1;
                }
            }
        }
        return returnMap;
    }

    private List<Object> ConvertMapToObjects(int[][] map)
    {
        List<Object> objects = new List<Object>();
        for(int y = 0; y < map.Length; y++)
        {
            for(int x = 0; x < map[0].Length; x++)
            {
                if (Random.NextDouble() < .9) continue;
                Object objectCreate = map[y][x] switch
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

    private int[][] GenerateBiomeTilemap() 
    {
        int[][] returnMap = new int[SizeCompact.Height][SizeCompact.Width];
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
        int[][] noiseMap = SectionedNoiceMap();
        float[][] smoothMap = SmoothMap(noiseMap, smoothRadius);

        for(int i = 0; i < repeatSmoothFunctions; i++)
        {
            smoothMap = SmoothMap(smoothMap, smoothRadius);
        }

        int[][] regionedMap = SeperateRegions(smoothMap);
        int[][] paddedMap = RegionPadding(regionedMap, regionPadding);
        List<Object> objects = ConvertMapToObjects(paddedMap);

        World world = new World()
        {
            Size = RealSize,
            Objects = objects,
            Players = new List<Player>()
        };
        return world;
    }
}