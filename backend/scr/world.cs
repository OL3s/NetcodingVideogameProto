using System.Collections.Generic;
using System.Dynamic;
using System.Security.Cryptography;

namespace BasicGameProject.Backend;

public class World
{
    public List<Player> Players = new List<Player>();
    public List<Object> Objects = new List<Object>();
    public Size Size { get; set; }
    public void Update()
    {
        
    }
}

public static class WorldGenerator
{
    public Size SizeCompact { get; set; }
    public int TileSize { get; set; }
    public Size RealSize => new Size(SizeCombact.X * TileSize, SizeCombat.Y * TileSize);
    public List<Object> Objects { get; set; }

    private bool[][] SectionedNoiceMap()
    {
        bool[SizeCompact.Height][SizeCompact.Width] returnMap = {}; 
        
        for(int y = 0; y < SizeCompact.Height; y++)
        {
            for(int x = 0; x < SizeCompact.Width; x++)
            {
                returnMap[y][x] = Random.NextFloat() < .5;
            }
        }
    }
}