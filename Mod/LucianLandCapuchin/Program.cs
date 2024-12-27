using LucianLandCapuchin;
using Swed64;
using System.Numerics;


// main logic

# region hooking and injecting
try
{
    Swed swed = new Swed("Capuchin");
    Console.WriteLine("Hooking to capuchin!");

    IntPtr client = swed.GetModuleBase("UnityPlayer.dll"); // if not work then use "GameAssembly.dll" or "baselib.dll"

    Renderer renderer = new Renderer();
    Thread renderThread = new Thread(new ThreadStart(renderer.Start().Wait));
    renderThread.Start();

    Vector2 ScreenSize = renderer.screenSize;

    Console.WriteLine("Hooked! Have fun!");
    Console.Beep(); 

    List<Entity> entities = new List<Entity>();
    Entity localplayer = new Entity();
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Error while initializing Swed: {ex.Message}");
}
#endregion
