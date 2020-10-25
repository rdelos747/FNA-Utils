using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Utils
{

  public class Renderer
  {


    public int Width { get; private set; }
    public int Height { get; private set; }
    public int ViewWidth { get; private set; }
    public int ViewHeight { get; private set; }
    public Matrix ScreenMatrix;
    public Camera Camera;
    public Node Root = new Node();
    public Effect CurrentEffect;

    public Renderer() : this(Engine.Width, Engine.Height) { }

    public Renderer(int width, int height)
    {
      Width = width;
      Height = height;
      Camera = new Camera(width, height);
      UpdateView();
    }

    public void ApplyEffect(Effect effect)
    {
      CurrentEffect = effect;
      Engine.SpriteBatch.End();

      Engine.SpriteBatch.Begin(
        SpriteSortMode.BackToFront,
        BlendState.AlphaBlend,
        SamplerState.PointClamp,
        DepthStencilState.None,
        RasterizerState.CullNone,
        CurrentEffect,
        Camera.Matrix * ScreenMatrix
      );
    }

    public void Draw()
    {
      CurrentEffect = null;

      Engine.SpriteBatch.Begin(
        SpriteSortMode.Deferred,
        BlendState.AlphaBlend,
        SamplerState.PointClamp,
        DepthStencilState.None,
        RasterizerState.CullNone,
        null,
        Camera.Matrix * ScreenMatrix
      );

      Root.Draw();

      Engine.SpriteBatch.End();
    }

    public void UpdateView()
    {
      GraphicsDevice graphics = Engine.Instance.GraphicsDevice;
      float screenWidth = graphics.PresentationParameters.BackBufferWidth;
      float screenHeight = graphics.PresentationParameters.BackBufferHeight;

      // get View Size
      if (screenWidth / Width > screenHeight / Height)
      {
        ViewWidth = (int)(screenHeight / Height * Width);
        ViewHeight = (int)screenHeight;
      }
      else
      {
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

    public void AddToRoot(Node n)
    {
      Root.AddChild(n);
    }

    public void RemoveFromRoot(Node n)
    {
      n.RemoveFromParent();
    }

    public virtual void Update()
    {
      if (Root.Active)
      {
        Root.Update();
      }
    }

    public Vector2 GetMouse()
    {
      return (Input.MousePos - new Vector2(Engine.Viewport.X, Engine.Viewport.Y)) / new Vector2(ScreenMatrix.M11, ScreenMatrix.M22);
    }
  }
}