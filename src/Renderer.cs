using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine {
  public class Renderer : Game {

    private List<GameObject> objects = new List<GameObject>();
    private bool isFullScreen = false;

    protected GraphicsDeviceManager graphics;
    protected SpriteBatch spriteBatch;

    public int count { get; private set; }

    public Renderer(int resWidth = 1280, int resHeight = 720, bool newFullScreen = false, bool allowResize = true) {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";

      isFullScreen = newFullScreen;

      Resolution.Init(ref graphics);
      Resolution.SetVirtualResolution(resWidth, resHeight);
      Resolution.SetResolution(resWidth, resHeight, isFullScreen);

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
      base.LoadContent();
    }

    override protected void Update(GameTime gameTime) {
      Input.update();
      updateObjects(gameTime);
      base.Update(gameTime);
    }

    protected virtual void updateObjects(GameTime gameTime) { }

    override protected void Draw(GameTime gameTime) {
      GraphicsDevice.Clear(Color.CornflowerBlue);

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

    void drawObjects() {
      for (int i = 0; i < objects.Count; i++) {
        objects[i].draw(spriteBatch);
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