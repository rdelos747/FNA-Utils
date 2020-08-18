using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Utils {

  public class Renderer {
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int ViewWidth { get; private set; }
    public int ViewHeight { get; private set; }
    public Matrix ScreenMatrix;
    public Camera Camera;
    public List<Node> Nodes = new List<Node>();

    public Renderer(GraphicsDevice graphics) : this(Engine.Width, Engine.Height, graphics) { }

    public Renderer(int width, int height, GraphicsDevice graphics) {
      Width = width;
      Height = height;
      Camera = new Camera(width, height);
      UpdateView(graphics);
    }

    public void Draw(SpriteBatch spriteBatch) {
      spriteBatch.Begin(
        SpriteSortMode.Deferred,
        BlendState.AlphaBlend,
        SamplerState.PointClamp,
        DepthStencilState.None,
        RasterizerState.CullNone,
        null,
        Camera.Matrix * ScreenMatrix
      );
      foreach (Node node in Nodes) {
        node.Draw(spriteBatch);
      }
      spriteBatch.End();
    }

    public void UpdateView(GraphicsDevice graphics) {
      float screenWidth = graphics.PresentationParameters.BackBufferWidth;
      float screenHeight = graphics.PresentationParameters.BackBufferHeight;

      // get View Size
      if (screenWidth / Width > screenHeight / Height) {
        ViewWidth = (int)(screenHeight / Height * Width);
        ViewHeight = (int)screenHeight;
      }
      else {
        ViewWidth = (int)screenWidth;
        ViewHeight = (int)(screenWidth / Width * Height);
      }

      // apply View Padding
      var aspect = ViewHeight / (float)ViewWidth;
      ViewWidth -= Engine.ViewPadding * 2;
      ViewHeight -= (int)(aspect * Engine.ViewPadding * 2);

      // update screen matrix
      ScreenMatrix = Matrix.CreateScale(ViewWidth / (float)Width, ViewWidth / (float)Width, 1);
    }

    public void Add(Node n) {
      Nodes.Add(n);
    }
  }
}