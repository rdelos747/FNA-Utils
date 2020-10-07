using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Utils
{

  public class Renderer
  {
    private float ShakeTime = 0;
    private Vector2 ShakePos = new Vector2();
    public float MinShake = 0.05f;    // stop shaking when ShakeTime is lower than this
    public float ShakeMult = 0.8f;    // multiply ShakeTime by this every frame
    public int ShakeAmt = 2;          // max pan distance the screen can shake

    public int Width { get; private set; }
    public int Height { get; private set; }
    public int ViewWidth { get; private set; }
    public int ViewHeight { get; private set; }
    public Matrix ScreenMatrix;
    public Camera Camera;
    public Node Root = new Node();
    public Effect CurrentEffect;

    public Renderer(GraphicsDevice graphics) : this(Engine.Width, Engine.Height, graphics) { }

    public Renderer(int width, int height, GraphicsDevice graphics)
    {
      Width = width;
      Height = height;
      Camera = new Camera(width, height);
      UpdateView(graphics);
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
        SpriteSortMode.BackToFront,
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

    public void UpdateView(GraphicsDevice graphics)
    {
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

    public void Update()
    {
      if (Root.Active)
      {
        Root.Update();
      }

      if (ShakeTime > 0.05f)
      {
        ShakePos.X = Rand.RandRange(-ShakeAmt, ShakeAmt) * ShakeTime;
        ShakePos.Y = Rand.RandRange(-ShakeAmt, ShakeAmt) * ShakeTime;
        ShakeTime *= ShakeMult;
      }
      else
      {
        ShakeTime = 0;
        ShakePos.X = 0;
        ShakePos.Y = 0;
      }

      Camera.Origin = ShakePos;
    }

    public void Shake(int power = 1)
    {
      ShakeTime = power;
    }
  }
}