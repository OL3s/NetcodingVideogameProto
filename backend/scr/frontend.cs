using Raylib_cs;

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
                            0 => Color.DarkGray,
                            1 => Color.Gray,
                            2 => Color.LightGray,
                            _ => Color.Magenta
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
            foreach (var obj in world.Objects)
            {
                Raylib.DrawEllipse(
                    (int)obj.Position.X - cameraPosition[0],
                    (int)obj.Position.Y - cameraPosition[1],
                    obj.CollisionSize.Width / 2f,
                    obj.CollisionSize.Height / 2f,
                    obj.Color
                );
            }

            // players
            foreach (var player in world.Players)
            {
                Raylib.DrawEllipse(
                    (int)player.Position.X - cameraPosition[0],
                    (int)player.Position.Y - cameraPosition[1],
                    player.CollisionSize.Width / 2f,
                    player.CollisionSize.Height / 2f,
                    player.Color
                );
            }

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