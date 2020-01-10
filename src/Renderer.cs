using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine {
  public class Renderer : Game {

    private List<GameObject> objects = new List<GameObject>();
    private bool isFullScreen = false;

    public static Texture2D systemRect { get; private set; }

    protected GraphicsDeviceManager graphics;
    protected SpriteBatch spriteBatch;

    public int count { get; private set; }

    public EngineState engineState;

    //protected Menu pauseMenu;
    protected Store<EngineDefaults.Settings> gameSettings;
    protected Menu pauseMenu;

    public Renderer(int resWidth, int resHeight, Color bkColor, bool startFullScreen, bool allowResize) {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";

      isFullScreen = startFullScreen;

      Resolution.Init(ref graphics, bkColor);
      Resolution.SetVirtualResolution(resWidth, resHeight);
      Resolution.SetResolution(resWidth, resHeight, isFullScreen);

      Window.AllowUserResizing = allowResize;
      Window.ClientSizeChanged += onResize;

      // The base constructor (this function) runs before derived,
      // so we can set the default input map here. If the derived
      // game wants to use a custom map, they can do so after this
      // runs, eg in their constructor.
      Input.setInputMap(EngineDefaults.inputMap);

      gameSettings = new Store<EngineDefaults.Settings>(EngineDefaults.reducer, new EngineDefaults.Settings());
    }

    private void onResize(Object sender, EventArgs e) {
      Rectangle resizedBounds = GraphicsDevice.Viewport.Bounds;
      //Resolution.SetVirtualResolution(resizedBounds.Width, resizedBounds.Height);
      Resolution.SetResolution(resizedBounds.Width, resizedBounds.Height, isFullScreen);
    }

    override protected void Initialize() {
      base.Initialize();
    }

    override protected void LoadContent() {
      spriteBatch = new SpriteBatch(GraphicsDevice);

      systemRect = new Texture2D(GraphicsDevice, 1, 1);
      systemRect.SetData(new Color[] { Color.White });

      base.LoadContent();

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
        SpriteSortMode.BackToFront,
        BlendState.AlphaBlend,
        SamplerState.LinearClamp,
        DepthStencilState.Default,
        RasterizerState.CullNone,
        null,
        Resolution.getTransformationMatrix()
      );

      drawObjects();
      spriteBatch.End();

      base.Draw(gameTime);
    }

    private void drawObjects() {
      for (int i = 0; i < objects.Count; i++) {
        GameObject obj = objects[i];
        obj.draw(spriteBatch);
      }
    }

    private void defaultUpdate() {
      if (Input.keyPressed(EngineDefaults.keyPause)) {
        if (engineState == EngineState.PAUSED) {
          engineState = EngineState.RUNNING;
          if (pauseMenu != null) {
            pauseMenu.close();
            pauseMenu = null;
          }
        }
        else if (engineState == EngineState.RUNNING) {
          engineState = EngineState.PAUSED;
          pauseMenu = new PauseMenu(this);
          pauseMenu.dispatch = gameSettings.dispatch;
          pauseMenu.init();
        }
      }

      if (Input.keyPressed(EngineDefaults.keyQuit)) {
        engineState = EngineState.QUIT;
      }

      if (pauseMenu != null) {
        pauseMenu.update();
      }
    }

    public void addObject(GameObject obj) {
      obj.init(this);
      obj.load(Content);
      objects.Add(obj);
      count = objects.Count;
    }

    public void removeObject(GameObject obj) {
      GameObject objToRemove = objects.SingleOrDefault(o => o == obj);
      if (objToRemove == null) {
        throw new System.ArgumentException("Cannot remove object from renderer - object not held by renderer");
      }
      objects.Remove(objToRemove);
      count = objects.Count;
    }
  }
}