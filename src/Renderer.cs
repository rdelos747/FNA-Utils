using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine {
  public static class Counter {
    public static int count;
  }

  public class Renderer : Game {

    protected GraphicsDeviceManager Graphics;
    protected SpriteBatch SpriteBatch;

    private Node Root;

    public EngineStates EngineState;
    public static Texture2D SystemRect { get; private set; }

    public static FontLib SystemFontLib;


    /*

TODO: if we use `EngineSettngs`, here would be the place to explain that. As of right now, there is no easy way to pass settings through...
- However, if some kind of settings object is used, it might be more straightforward to pass that to `Initialize`

    */

    public Renderer() {
      Graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
      TextureLoader.Content = Content;
    }

    private void onResize(Object sender, EventArgs e) {
      Rectangle resizedBounds = GraphicsDevice.Viewport.Bounds;
      Resolution.SetResolution(resizedBounds.Width, resizedBounds.Height, EngineDefaults.FullScreen);
    }

    protected virtual void Initialize(EngineSettings settings) {
      /*
      Initialize any values that are based on EngineSettings
      */
      Resolution.Init(ref Graphics, settings.BackgroundColor);
      Resolution.SetVirtualResolution(settings.VirtualWidth, settings.VirtualHeight);
      Resolution.SetResolution(settings.ScreenWidth, settings.ScreenHeight, settings.StartFullscreen);

      Window.AllowUserResizing = settings.AllowResize;
      Window.ClientSizeChanged += onResize;
      IsMouseVisible = settings.StartMouseVisible;

      Input.SetInputMap(settings.InputMap);

      SystemFontLib = new FontLib(settings.SystemFontPath, GraphicsDevice);

      Root = new Node();
      Root.Bounds = new Rectangle(0, 0, EngineDefaults.Width, EngineDefaults.Height);

      base.Initialize();
    }

    override protected void LoadContent() {
      SpriteBatch = new SpriteBatch(GraphicsDevice);

      /*
      Set they system rectangle, which is used for drawing bounds. 
      */
      SystemRect = new Texture2D(GraphicsDevice, 1, 1);
      SystemRect.SetData(new Color[] { Color.White });

      base.LoadContent();

      EngineState = EngineStates.RUNNING;
    }

    override protected void Update(GameTime gameTime) {
      Input.update();

      UpdateGame(gameTime);

      if (EngineState == EngineStates.QUIT) {
        this.Exit();
      }

      base.Update(gameTime);
    }

    protected virtual void UpdateGame(GameTime gameTime) { }

    override protected void Draw(GameTime gameTime) {

      Resolution.BeginDraw();
      SpriteBatch.Begin(
        SpriteSortMode.Deferred,
        BlendState.AlphaBlend,
        SamplerState.LinearClamp,
        DepthStencilState.Default,
        RasterizerState.CullNone,
        null,
        Resolution.getTransformationMatrix()
      );

      Root.Draw(SpriteBatch, 0, 0);

      SpriteBatch.End();

      base.Draw(gameTime);
    }

    public void AddChildToRoot(Node n) {
      Root.AddChild(n);
    }
  }
}