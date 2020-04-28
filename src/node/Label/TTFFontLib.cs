using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpFont;

namespace Utils {
  public sealed class TTFFontLib {
    public Library library { get; private set; } = new Library();
    private readonly GraphicsDevice graphicsDevice;

    private readonly Dictionary<int, TTFFont> fonts = new Dictionary<int, TTFFont>();

    private readonly byte[] fontBytes;

    public TTFFontLib(string fontPath, GraphicsDevice graphics) {
      Stream fileStream = File.OpenRead(fontPath);
      MemoryStream fontMs = new MemoryStream();
      fileStream.CopyTo(fontMs);
      fontBytes = fontMs.ToArray();

      graphicsDevice = graphics;
      library = new Library();
    }

    public TTFFont CreateFont(int size) {
      if (!fonts.TryGetValue(size, out var font)) {
        font = new TTFFont(this, size, fontBytes, graphicsDevice);
      }
      return font;
    }
  }

  public sealed class TTFFont : Font {
    private readonly GraphicsDevice Graphics;
    public int FontSize { get; private set; }
    public bool hasKerning { get; private set; }

    private TTFFontLib Parent;
    private Face Face;
    private byte[] FontBytes;

    public TTFFont(TTFFontLib parent, int size, byte[] fontBytes, GraphicsDevice graphics) {
      Graphics = graphics;
      Parent = parent;
      FontSize = size;
      FontBytes = fontBytes;
      Face = new Face(parent.library, fontBytes, 0);

      Face.SetCharSize(0, size, 0, 96); // not sure why the other params besides size..
      LineHeight = Face.Size.Metrics.NominalHeight;
      Ascender = (int)Face.Size.Metrics.Ascender;
      hasKerning = Face.HasKerning;

      // load an initial glyph into the glyphslot so we
      // can get the advance space
      Face.LoadGlyph(Face.GetCharIndex(32), LoadFlags.Default, LoadTarget.Normal);
      Advance = (int)Face.Glyph.Metrics.HorizontalAdvance;
    }

    public override Glyph GetGlyph(char c) {
      Glyph glyph;
      if (!Glyphs.TryGetValue(c, out glyph)) {
        uint glyphIndex = Face.GetCharIndex(c);
        if (glyphIndex == 0) {
          return null;
        }

        byte[] bufferData;
        Face.LoadGlyph(glyphIndex, LoadFlags.Default, LoadTarget.Normal);
        if (Face.Glyph.Metrics.Width == 0) {
          bufferData = new Byte[0];
        }
        else {
          Face.Glyph.RenderGlyph(RenderMode.Normal);
          bufferData = Face.Glyph.Bitmap.BufferData;
        }
        glyph = new TTFGlyph(c, glyphIndex, bufferData, Face.Glyph.Metrics, Face.Glyph.BitmapLeft, Graphics);
        Glyphs.Add(c, glyph);
      }

      return glyph;
    }

    // public FTVector26Dot6 getKerning(uint l, uint r) {
    //   return face.GetKerning(l, r, KerningMode.Default);
    // }
  }

  public sealed class TTFGlyph : Glyph {
    // https://www.freetype.org/freetype2/docs/glyphs/glyphs-3.html
    // https://github.com/Robmaister/SharpFont/blob/master/Source/SharpFont/GlyphSlot.cs

    public TTFGlyph(char c, uint index, byte[] bufferData, GlyphMetrics metrics, int bitmapLeft, GraphicsDevice graphics) {
      C = c;
      Index = index;
      Width = (int)metrics.Width;
      Height = (int)metrics.Height;
      AdvanceX = (int)metrics.HorizontalAdvance;
      BearingX = (int)metrics.HorizontalBearingX;
      BearingY = (int)metrics.HorizontalBearingY;
      BitmapLeft = bitmapLeft;

      // turn buffer data into texture
      Texture = new Texture2D(graphics, Width, Height);
      Color[] colors = new Color[Width * Height];
      for (int j = 0; j < Height; j++) {
        for (int i = 0; i < Width; i++) {
          var src = (j * Width) + i;
          colors[src] = Color.FromNonPremultiplied(255, 255, 255, bufferData[src]);
        }
      }
      Texture.SetData(colors);
      Clip = new Rectangle(0, 0, Texture.Width, Texture.Height);
    }
  }
}