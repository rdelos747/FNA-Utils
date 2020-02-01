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

    //private FontLib systemFontLib;
    //private Font systemFont;
    private TextObject fpsCounter;

    protected GraphicsDeviceManager graphics;
    protected SpriteBatch spriteBatch;
    //protected EngineSettings engineSettings;
    protected Element pauseMenu;

    private Node root;

    public EngineState engineState;
    public static Texture2D systemRect { get; private set; }

    public Renderer() {
      //engineSettings = new EngineSettings(this);
      Input.setInputMap(EngineDefaults.inputMap);

      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
      TextureLoader.Content = Content;

      Resolution.Init(ref graphics, Color.Black);
      Resolution.SetVirtualResolution(EngineDefaults.width, EngineDefaults.height);
      Resolution.SetResolution(EngineDefaults.width, EngineDefaults.height, EngineDefaults.fullScreen);

      Window.AllowUserResizing = EngineDefaults.allowResize;
      Window.ClientSizeChanged += onResize;
      IsMouseVisible = EngineDefaults.mouseVisible;

      root = new Node();
      root.Bounds = new Rectangle(0, 0, EngineDefaults.width, EngineDefaults.height);
    }

    private void onResize(Object sender, EventArgs e) {
      Rectangle resizedBounds = GraphicsDevice.Viewport.Bounds;
      Resolution.SetResolution(resizedBounds.Width, resizedBounds.Height, EngineDefaults.fullScreen);
    }

    override protected void Initialize() {
      base.Initialize();
    }

    override protected void LoadContent() {
      spriteBatch = new SpriteBatch(GraphicsDevice);

      systemRect = new Texture2D(GraphicsDevice, 1, 1);
      systemRect.SetData(new Color[] { Color.White });

      base.LoadContent();

      EngineDefaults.SystemFontLibrary = new FontLib(EngineDefaults.fontPath, GraphicsDevice);
      EngineDefaults.SystemFontReg = EngineDefaults.SystemFontLibrary.createFont(EngineDefaults.fontSizeReg);
      EngineDefaults.SystemFontLarge = EngineDefaults.SystemFontLibrary.createFont(EngineDefaults.fontSizeLarge);
      //fpsCounter = new TextObject(systemFont);

      engineState = EngineState.RUNNING;
    }

    override protected void Update(GameTime gameTime) {
      Input.update();
      defaultUpdate();

      if (engineState == EngineState.QUIT) {
        this.Exit();
      }

      if (engineState == EngineState.RUNNING) {
        updateGame(gameTime);
      }
      base.Update(gameTime);
    }

    protected virtual void updateGame(GameTime gameTime) { }

    override protected void Draw(GameTime gameTime) {

      Resolution.BeginDraw();
      spriteBatch.Begin(
        //SpriteSortMode.BackToFront,
        SpriteSortMode.Deferred,
        BlendState.AlphaBlend,
        SamplerState.LinearClamp,
        DepthStencilState.Default,
        RasterizerState.CullNone,
        null,
        Resolution.getTransformationMatrix()
      );

      root.Draw(spriteBatch, 0, 0);

      spriteBatch.End();

      base.Draw(gameTime);
    }

    private void defaultUpdate() {
      if (Input.keyPressed(EngineDefaults.keyPause)) {
        if (engineState == EngineState.PAUSED) {
          engineState = EngineState.RUNNING;
          pauseMenu.RemoveFromParent();
        }
        else if (engineState == EngineState.RUNNING) {
          engineState = EngineState.PAUSED;
          pauseMenu = new PauseMenu(this);
        }
      }

      if (Input.keyPressed(EngineDefaults.keyQuit)) {
        engineState = EngineState.QUIT;
      }

      if (pauseMenu != null) {
        pauseMenu.Update(Input.mouseX, Input.mouseY);
      }
    }

    public void AddChildToRoot(Node n) {
      root.AddChild(n);
    }
  }
}