// backend/Program.cs
using BasicGameProject.Backend;
using BasicGameProject.Backend.FrontendDebugger;

using System.Threading;

World world = new World(new Size(50, 30));
world.GenerateWorld();
Frontend frontend = new Frontend(world);

using var cts = new CancellationTokenSource();

var frontendThread = new Thread(() =>
{
	try
	{
		frontend.Start(cts.Token);
	}
	finally
	{
		// If the window closes, signal the rest of the app to stop.
		cts.Cancel();
	}
});

var backendThread = new Thread(() =>
{
	while (!cts.Token.IsCancellationRequested)
	{
		// Backend logic would go here
		Thread.Sleep(100); // Simulate work
	}
});

frontendThread.IsBackground = true;
frontendThread.Start();

// Backend setup / debug output
frontendThread.Join();