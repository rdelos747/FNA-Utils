using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Engine {
  public class Renderer : Game {

    private List<GameObject> objects = new List<GameObject>();
    private bool isFullScreen = false;

    private Texture2D boundsTest;

    protected GraphicsDeviceManager graphics;
    protected SpriteBatch spriteBatch;

    public Dictionary<string, Keys> inputMap { get; private set; }

    public int count { get; private set; }

    public EngineState engineState;

    public Renderer(int resWidth, int resHeight, Color bkColor, bool startFullScreen, bool allowResize, Dictionary<string, Keys> inMap) {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";

      isFullScreen = startFullScreen;

      Resolution.Init(ref graphics, bkColor);
      Resolution.SetVirtualResolution(resWidth, resHeight);
      Resolution.SetResolution(resWidth, resHeight, isFullScreen);

      inputMap = inMap;

      Window.AllowUserResizing = allowResize;
      Window.ClientSizeChanged += onResize;
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

      boundsTest = new Texture2D(GraphicsDevice, 1, 1);
      boundsTest.SetData(new Color[] { Color.White });

      base.LoadContent();

      engineState = EngineState.RUNNING;
    }

    override protected void Update(GameTime gameTime) {
      Input.update();
      if (engineState == EngineState.RUNNING) {
        updateObjects(gameTime);
      }
      base.Update(gameTime);
    }

    protected virtual void updateObjects(GameTime gameTime) { }

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

      drawObjects(gameTime);
      spriteBatch.End();

      base.Draw(gameTime);
    }

    void drawObjects(GameTime gameTime) {
      for (int i = 0; i < objects.Count; i++) {
        GameObject obj = objects[i];
        obj.draw(spriteBatch, gameTime);
        if (obj.bounds != null && obj.showBounds) {
          int ox1 = (int)(obj.x + obj.bounds.X);
          int oy1 = (int)(obj.y + obj.bounds.Y);
          spriteBatch.Draw(boundsTest, new Rectangle(ox1, oy1, obj.bounds.Width, obj.bounds.Height), Color.Blue * 0.5f);
        }
      }
    }

    private Dictionary<string, Keys> getDefaultInputMap() {
      Dictionary<string, Keys> temp = new Dictionary<string, Keys>();
      temp.Add("primary", Keys.X);
      temp.Add("secondary", Keys.Z);
      temp.Add("up", Keys.Up);
      temp.Add("down", Keys.Down);
      temp.Add("left", Keys.Left);
      temp.Add("right", Keys.Right);
      temp.Add("pause", Keys.Escape);
      return temp;
    }

    protected void setInputMapKey(string key, Keys val) {
      inputMap[key] = val;
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