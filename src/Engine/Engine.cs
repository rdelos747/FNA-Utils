using System;
using System.IO;
using System.Runtime;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SDL2;

namespace Utils
{

  public partial class Engine : Game
  {
    /*
    Instance
    */
    public static Engine Instance { get; private set; }

    /*
    Graphics
    */
    private GraphicsDeviceManager Graphics;
    public static SpriteBatch SpriteBatch { get; private set; }
    public static Texture2D SystemRect { get; private set; }
    public static Color ClearColor;
    public static Viewport Viewport { get; private set; }
    public static ContentManager ContentRef;

    /*
    Screen size
    */
    public static int Width { get; private set; } // virtual
    public static int Height { get; private set; } // virtual
    public static int ViewWidth { get; private set; }
    public static int ViewHeight { get; private set; }
    private static int viewPadding = 0;
    public static int ViewPadding
    {
      get
      {
        return viewPadding;
      }
      set
      {
        viewPadding = value;
        Instance.UpdateView();
      }
    }
    private static bool resizing;

    /* 
    Content directory
    */
#if !CONSOLE
    private static string AssemblyDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
#endif

    public static string ContentDirectory
    {
#if PS4
            get { return Path.Combine("/app0/", Instance.Content.RootDirectory); }
#elif NSWITCH
            get { return Path.Combine("rom:/", Instance.Content.RootDirectory); }
#elif XBOXONE
            get { return Instance.Content.RootDirectory; }
#else
      get { return Path.Combine(AssemblyDirectory, Instance.Content.RootDirectory); }
#endif
    }

    /*
    Nodes
    */
    internal static Renderer CurrentRenderer;

    private Scene _scene;
    public Scene Scene
    {
      get
      {
        return _scene;
      }
      set
      {
        if (_scene != null)
        {
          _scene.End();
        }

        _scene = value;
        _scene.Start();
      }
    }

    /*
    Time
    */
    public static int FPS;
    private TimeSpan counterElapsed = TimeSpan.Zero;
    private int fpsCounter = 0;
    public static float DeltaTime { get; private set; }
    public static float RawDeltaTime { get; private set; }
    public static float TimeRate = 1f;

    /*
    Other
    */
    public static bool ExitOnEscapeKeypress;
    public string Title;

    public Engine(int width, int height)
    {
      Instance = this;
      Width = width;
      Height = height;

      /*
      Things that will come from loading settings
      */
      bool fullscreen = false;
      int windowWidth = 1280;
      int windowHeight = 720;
      ClearColor = Color.Black;

      Graphics = new GraphicsDeviceManager(this);
      Graphics.DeviceReset += OnGraphicsReset;
      Graphics.DeviceCreated += OnGraphicsCreate;
      Graphics.SynchronizeWithVerticalRetrace = false;
      Graphics.PreferMultiSampling = false;
      Graphics.GraphicsProfile = GraphicsProfile.HiDef;
      Graphics.PreferredBackBufferFormat = SurfaceFormat.Color;
      Graphics.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
      Graphics.IsFullScreen = false;
      Graphics.ApplyChanges();

#if PS4 || XBOXONE
            Graphics.PreferredBackBufferWidth = 1920;
            Graphics.PreferredBackBufferHeight = 1080;
#elif NSWITCH
            Graphics.PreferredBackBufferWidth = 1280;
            Graphics.PreferredBackBufferHeight = 720;
#else
      Window.AllowUserResizing = true;
      Window.ClientSizeChanged += OnClientSizeChanged;

      if (fullscreen)
      {
        Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        Graphics.IsFullScreen = true;
      }
      else
      {
        Graphics.PreferredBackBufferWidth = windowWidth;
        Graphics.PreferredBackBufferHeight = windowHeight;
        Graphics.IsFullScreen = false;
      }
#endif

      Content.RootDirectory = @"Content";
      ContentRef = Content;

      IsMouseVisible = false;
      IsFixedTimeStep = true;
      ExitOnEscapeKeypress = true;

      GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
    }

#if !CONSOLE
    protected virtual void OnClientSizeChanged(object sender, EventArgs e)
    {
      if (Window.ClientBounds.Width > 0 && Window.ClientBounds.Height > 0 && !resizing)
      {
        resizing = true;

        Graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
        Graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
        UpdateView();

        resizing = false;
      }
    }
#endif

    protected override void LoadContent()
    {
      Console.WriteLine("Load Content");

      SpriteBatch = new SpriteBatch(GraphicsDevice);

      SystemRect = new Texture2D(GraphicsDevice, 1, 1);
      SystemRect.SetData(new Color[] { Color.White });

      Screenshot.Graphics = GraphicsDevice;
      Screenshot.DirName = "./screenshots";
      Screenshot.Prefix = "sc_";

      Label.BaseFont = new Atlas("aseprite_font.png", 16, 14);

      base.LoadContent();
    }

    protected override void Initialize()
    {
      Console.WriteLine("Initialize");
      Input.Init();
      base.Initialize();

      SDL.SDL_SetEventFilter(_sdlIntercept, IntPtr.Zero);
    }

    protected override void Update(GameTime gameTime)
    {
      RawDeltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
      DeltaTime = RawDeltaTime * TimeRate;

      Input.Update();

#if !CONSOLE
      if (ExitOnEscapeKeypress && Input.KeyboardState.IsKeyDown(Keys.Escape))
      {
        Exit();
        return;
      }

      if (Screenshot.Key != Keys.None && Input.KeyboardState.IsKeyDown(Screenshot.Key))
      {
        string info = $"";
        Screenshot.Capture(info);
      }
#endif

      _scene.Update();

      base.Update(gameTime);
    }

    override protected void Draw(GameTime gameTime)
    {
      GraphicsDevice.SetRenderTarget(null);
      GraphicsDevice.Viewport = Viewport;
      GraphicsDevice.Clear(ClearColor);

      // foreach (Renderer renderer in Renderers)
      // {
      //   CurrentRenderer = renderer;
      //   renderer.Draw();
      // }

      _scene.Draw();

      base.Draw(gameTime);

      /*
      Frame counter
      */
      fpsCounter++;
      counterElapsed += gameTime.ElapsedGameTime;
      if (counterElapsed >= TimeSpan.FromSeconds(1))
      {
#if DEBUG
        Window.Title = Title + " " + fpsCounter.ToString() + " fps - " + DeltaTime + " dt - " + (GC.GetTotalMemory(false) / 1048576f).ToString("F") + " MB";
#endif
        FPS = fpsCounter;
        fpsCounter = 0;
        counterElapsed -= TimeSpan.FromSeconds(1);
      }
    }

    protected override void OnExiting(object sender, EventArgs args)
    {
      base.OnExiting(sender, args);
    }

    private static SDL.SDL_EventFilter _sdlIntercept = SDLIntercept;
    private static unsafe int SDLIntercept(IntPtr func, IntPtr evtPtr)
    {
      SDL.SDL_Event* evt = (SDL.SDL_Event*)evtPtr;
      if (evt->type == SDL.SDL_EventType.SDL_QUIT)
      {
        return Instance.TrapExit();
      }
      return 1;
    }

    protected virtual int TrapExit()
    {
      return 1;
    }

    public static Texture2D LoadTexture(string path)
    {
      return ContentRef.Load<Texture2D>(path);
    }

    public static Effect LoadEffect(string path)
    {
      return ContentRef.Load<Effect>(path);
    }
  }
}