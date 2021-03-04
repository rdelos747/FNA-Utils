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
    public Scene Scene;

    public SpriteSortMode SortMode = SpriteSortMode.Immediate;
    private RasterizerState RasterizerState = RasterizerState.CullNone;
    private Rectangle _cropRectScaled = Rectangle.Empty;
    private Rectangle _cropRect = Rectangle.Empty;
    public Rectangle CropRect
    {
      get
      {
        return _cropRect;
      }
      set
      {
        _cropRect = value;
        UpdateView();
      }
    }
    private Rectangle OriginalRect;
    private bool _cropped = false;
    public bool Cropped
    {
      get
      {
        return _cropped;
      }
      set
      {
        _cropped = value;
        if (_cropped)
        {
          RasterizerState.ScissorTestEnable = true;
        }
        else
        {
          RasterizerState.ScissorTestEnable = false;
        }
      }
    }


    public Renderer() : this(Engine.Width, Engine.Height) { }

    public Renderer(int width, int height)
    {
      Width = width;
      Height = height;
      Camera = new Camera(width, height);
      UpdateView();
    }

    public virtual void ApplyEffect(Effect effect)
    {
      CurrentEffect = effect;

      if (Cropped)
      {
        Engine.SpriteBatch.GraphicsDevice.ScissorRectangle = OriginalRect;
      }

      Engine.SpriteBatch.End();

      if (Cropped)
      {
        Engine.SpriteBatch.Begin(
          SpriteSortMode.Immediate,
          BlendState.AlphaBlend,
          SamplerState.PointClamp,
          DepthStencilState.None,
          RasterizerState,
          CurrentEffect,
          Camera.Matrix * ScreenMatrix
        );

        Engine.SpriteBatch.GraphicsDevice.ScissorRectangle = _cropRectScaled;
      }
      else
      {
        Engine.SpriteBatch.Begin(
          SortMode,
          BlendState.AlphaBlend,
          SamplerState.PointClamp,
          DepthStencilState.None,
          RasterizerState,
          CurrentEffect,
          Camera.Matrix * ScreenMatrix
        );
      }
    }

    public virtual void Draw()
    {
      CurrentEffect = null;

      if (Cropped)
      {
        Engine.SpriteBatch.Begin(
          SpriteSortMode.Immediate, // for some reason this needs to be immediate :/
          BlendState.AlphaBlend,
          SamplerState.PointClamp,
          DepthStencilState.None,
          RasterizerState,
          null,
          Camera.Matrix * ScreenMatrix
        );

        Engine.SpriteBatch.GraphicsDevice.ScissorRectangle = _cropRectScaled;
      }
      else
      {
        Engine.SpriteBatch.Begin(
          SortMode,
          BlendState.AlphaBlend,
          SamplerState.PointClamp,
          DepthStencilState.None,
          RasterizerState,
          null,
          Camera.Matrix * ScreenMatrix
        );
      }

      Root.Draw();

      if (Cropped)
      {
        Engine.SpriteBatch.GraphicsDevice.ScissorRectangle = OriginalRect;
      }

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

      // Save the original crop rectangle
      OriginalRect = Engine.SpriteBatch.GraphicsDevice.ScissorRectangle;

      // scale the crop rectangle
      _cropRectScaled = new Rectangle(
        (int)(_cropRect.X * ScreenMatrix.M11),
        (int)(_cropRect.Y * ScreenMatrix.M22),
        (int)(_cropRect.Width * ScreenMatrix.M11),
        (int)(_cropRect.Height * ScreenMatrix.M22)
      );
    }

    public void AddToRoot(Node n)
    {
      Root.AddChild(n);
    }

    public void RemoveFromRoot(Node n)
    {
      n.RemoveFromParent();
    }

    public void RemoveFromScene()
    {
      if (Scene != null)
      {
        Scene.Renderers.Remove(this);
        Scene = null;
      }

      for (int i = Root.Nodes.Count - 1; i >= 0; i--)
      {
        Root.Nodes[i].RemoveFromParent();
      }

      Console.WriteLine("Removed renderer");
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
      return (Input.MousePos - new Vector2(Engine.Viewport.X, Engine.Viewport.Y)) / new Vector2(ScreenMatrix.M11, ScreenMatrix.M22) - Camera.Origin;
    }
  }
}