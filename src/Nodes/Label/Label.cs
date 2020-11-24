using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpFont;

namespace Utils
{

  public enum VerticalAlignment
  {
    Top,
    Center,
    Bottom
  }

  public enum HorizontalAlignment
  {
    Left,
    Center,
    Right
  }

  public class Label : Node
  {

    public static Font BaseFont;

    private Font _font;
    public Font Font
    {
      get
      {
        return _font;
      }
      set
      {
        _font = value;
        Size = new Size(10, _font.LineHeight);

        if (_text != null)
        {
          SetText(_text);
        }
      }
    }

    protected List<(char c, Vector2 loc)> Points;

    private string _text;
    public string Text
    {
      get
      {
        return _text;
      }
      set
      {
        SetText(value);
      }
    }

    public Vector2 TextOrigin = new Vector2();
    private VerticalAlignment _verticalAlignment;
    public VerticalAlignment VerticalAlignment
    {
      get
      {
        return _verticalAlignment;
      }
      set
      {
        _verticalAlignment = value;

        if (_font != null)
        {
          Size = new Size(0, _font.LineHeight);
        }

        if (_text != null)
        {
          SetText(_text);
        }
      }
    }
    private HorizontalAlignment _horizontalAlignment;
    public HorizontalAlignment HorizontalAlignment
    {
      get
      {
        return _horizontalAlignment;
      }
      set
      {
        _horizontalAlignment = value;


        if (_font != null)
        {
          Size = new Size(0, _font.LineHeight);
        }

        if (_text != null)
        {
          SetText(_text);
        }
      }
    }

    public Label(Font font = null) : this(null, 0, 0, HorizontalAlignment.Left, VerticalAlignment.Top, font) { }

    public Label(string text, int x, int y) : this(text, x, y, HorizontalAlignment.Left, VerticalAlignment.Top, null) { }

    public Label(string text, int x, int y, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, Font font = null)
    {
      _font = font;
      if (_font == null)
      {
        _font = BaseFont;
      }

      Size = new Size(0, _font.LineHeight);

      Points = new List<(char, Vector2)>();

      Position.X = x;
      Position.Y = y;
      _horizontalAlignment = horizontalAlignment;
      _verticalAlignment = verticalAlignment;
      if (text != null)
      {
        SetText(text);
      }
      else
      {
        SetText("");
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing == true)
      {
        // image.Dispose();
        // TODO:
        // finish this at some point
      }
    }

    protected override void Render()
    {
      for (int i = 0; i < Points.Count; i++)
      {
        Glyph glyph = _font.GetGlyph(Points[i].Item1);
        Engine.SpriteBatch.Draw(
          glyph.Texture,
          new Vector2(
            (Points[i].loc.X + DrawPosition.X),
            (Points[i].loc.Y + DrawPosition.Y)
          ),
          glyph.Clip,
          Color,
          0.0f,
          TextOrigin,
          1,
          SpriteEffects.None,
          Depth
        );
      }
    }

    public override void Update()
    {
      base.Update();
    }

    public void SetText(string t)
    {
      // modfied version of 
      // https://bitbucket.org/jacobalbano/fnt/src/default/
      _text = t;

      Points.Clear();

      if (_text.Length == 0)
      {
        return;
      }

      // loop over every char and position its glyph
      int px = 0;
      int py = 0;
      int maxWidth = 0;
      int maxHeight = 0;
      for (int i = 0; i < _text.Length; i++)
      {
        char c = _text[i];

        // do special char stuff
        switch (c)
        {
          case ' ':
            px += _font.Advance;
            continue;
          case '\t':
            px += _font.Advance * _font.Tab;
            continue;
          case '\r':  //  ignore
            continue;
          case '\n':
            px = 0;
            py += _font.LineHeight;
            continue;
        }

        Glyph glyph = _font.GetGlyph(c);
        if (glyph == null)
        {
          glyph = _font.GetGlyph('?');
        }

        int gx = px + glyph.BitmapLeft;
        int gy = py + _font.Ascender - glyph.BearingY;

        Points.Add((glyph.C, new Vector2(gx, gy)));
        px += glyph.AdvanceX;

        maxWidth = Math.Max(maxWidth, px);
        maxHeight = Math.Max(maxHeight, py + glyph.Height);
      }

      Size = new Size(maxWidth, maxHeight);

      TextOrigin = Vector2.Zero;
      if (_horizontalAlignment == HorizontalAlignment.Center)
      {
        TextOrigin.X = (float)Math.Ceiling(Size.Width / 2);
      }
      else if (_horizontalAlignment == HorizontalAlignment.Right)
      {
        TextOrigin.X = Size.Width;
      }

      if (_verticalAlignment == VerticalAlignment.Center)
      {
        TextOrigin.Y = (int)Size.Height / 2;
      }
      else if (_verticalAlignment == VerticalAlignment.Bottom)
      {
        TextOrigin.Y = (int)Size.Height;
      }
    }
  }
}