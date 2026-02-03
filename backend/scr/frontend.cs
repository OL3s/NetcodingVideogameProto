using Raylib_cs;
using System.Threading;

namespace BasicGameProject.Backend.FrontendDebugger;

public class Frontend
{
    private readonly World world;
    public Frontend(World world)
    {
        this.world = world;
    }
    public void Start(CancellationToken cancellationToken = default)
    {
        Raylib.InitWindow(800, 450, "BasicGameProject");
        Raylib.SetTargetFPS(60);

        int[] cameraPosition = { 0, 0 };

        while (!Raylib.WindowShouldClose() && !cancellationToken.IsCancellationRequested)
        {
            Raylib.BeginDrawing();

            // background
            Raylib.ClearBackground(new Color(135, 206, 235, 255)); // Sky blue background

            // gridded background
            var tileMap = world.TileMap;
            if (tileMap != null)
            {
                int rows = tileMap.GetLength(0);
                int cols = tileMap.GetLength(1);

                for (int y = 0; y < rows; y++)
                {
                    for (int x = 0; x < cols; x++)
                    {
                        int px = x * world.TileSize;
                        int py = y * world.TileSize;
                        int size = Math.Max(1, world.TileSize);
                        int tile = tileMap[y, x];

                        Color color = tile switch
                        {
                            0 => Color.DarkGreen,
                            1 => Color.Green,
                            _ => Color.Brown
                        };

                        Raylib.DrawRectangle(
                            px - cameraPosition[0],
                            py - cameraPosition[1],
                            size,
                            size,
                            color
                        );
                    }
                }
            }
            else
            {
                Raylib.DrawText("TileMap is null", 10, 30, 20, Color.Maroon);
            }

            // objects

            // players

            // overlay U
            Raylib.DrawFPS(10, 10);

            // camera movement
            if (Raylib.IsKeyDown(KeyboardKey.Right))
                cameraPosition[0] += 5;
            if (Raylib.IsKeyDown(KeyboardKey.Left))
                cameraPosition[0] -= 5;
            if (Raylib.IsKeyDown(KeyboardKey.Down))
                cameraPosition[1] += 5;
            if (Raylib.IsKeyDown(KeyboardKey.Up))
                cameraPosition[1] -= 5;
            Raylib.EndDrawing();
        }
        Raylib.CloseWindow();
    }
}