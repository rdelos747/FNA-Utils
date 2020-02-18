using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpFont;

namespace Engine {
  public sealed class FontLib {
    public Library library { get; private set; } = new Library();
    private readonly GraphicsDevice graphicsDevice;

    private readonly Dictionary<int, Font> fonts = new Dictionary<int, Font>();

    private readonly byte[] fontBytes;

    public FontLib(string fontPath, GraphicsDevice graphics) {
      Stream fileStream = File.OpenRead(fontPath);
      MemoryStream fontMs = new MemoryStream();
      fileStream.CopyTo(fontMs);
      fontBytes = fontMs.ToArray();

      graphicsDevice = graphics;
      library = new Library();
    }

    public Font CreateFont(int size) {
      if (!fonts.TryGetValue(size, out var font)) {
        font = new Font(this, size, fontBytes, graphicsDevice);
      }
      return font;
    }
  }

  public sealed class Font {
    private readonly GraphicsDevice graphicsDevice;
    public int size { get; private set; }
    public int ascender { get; private set; }
    public int advanceSpace { get; private set; }
    public int tabSpaces;
    public int lineHeight;
    public bool hasKerning { get; private set; }

    private FontLib parent;
    private Face face;
    private byte[] fontBytes;

    public Dictionary<char, GlyphData> glyphs;
    //public Dictionary<char, Dictionary<char, int>> kerning;

    public Font(FontLib parent, int size, byte[] fontBytes, GraphicsDevice graphics) {
      glyphs = new Dictionary<char, GlyphData>();
      //kerning = new Dictionary<char, Dictionary<char, int>>();

      graphicsDevice = graphics;
      this.parent = parent;
      this.size = size;
      this.fontBytes = fontBytes;
      face = new Face(parent.library, fontBytes, 0);

      face.SetCharSize(0, size, 0, 96); // not sure why the other params besides size..
      lineHeight = face.Size.Metrics.NominalHeight;
      ascender = (int)face.Size.Metrics.Ascender;
      hasKerning = face.HasKerning;

      // load an initial glyph into the glyphslot so we
      // can get the advance space
      face.LoadGlyph(face.GetCharIndex(32), LoadFlags.Default, LoadTarget.Normal);
      advanceSpace = (int)face.Glyph.Metrics.HorizontalAdvance;
    }

    public GlyphData getGlyph(char c) {
      GlyphData glyph;
      if (!glyphs.TryGetValue(c, out glyph)) {
        uint glyphIndex = face.GetCharIndex(c);
        if (glyphIndex == 0) {
          return null;
        }

        byte[] bufferData;
        face.LoadGlyph(glyphIndex, LoadFlags.Default, LoadTarget.Normal);
        if (face.Glyph.Metrics.Width == 0) {
          bufferData = new Byte[0];
        }
        else {
          face.Glyph.RenderGlyph(RenderMode.Normal);
          bufferData = face.Glyph.Bitmap.BufferData;
        }
        glyph = new GlyphData(c, glyphIndex, bufferData, face.Glyph.Metrics, face.Glyph.BitmapLeft, graphicsDevice);
        glyphs.Add(c, glyph);
      }

      return glyph;
    }

    // public FTVector26Dot6 getKerning(uint l, uint r) {
    //   return face.GetKerning(l, r, KerningMode.Default);
    // }
  }

  public sealed class GlyphData {
    // https://www.freetype.org/freetype2/docs/glyphs/glyphs-3.html
    // https://github.com/Robmaister/SharpFont/blob/master/Source/SharpFont/GlyphSlot.cs
    public readonly char c;
    public readonly uint index;
    public readonly Texture2D texture;
    public readonly int width;
    public readonly int height;
    // Bitmaps left bearing, is this needed?
    public readonly int bitmapLeft;
    // Horizontal distance to increment after rendering glyph
    public readonly int advanceX;
    // Horizontal distance from pen position to glyphs left bbox edge
    public readonly int bearingX;
    // Vertical distance from baseline to top of glyphs bbx
    public readonly int bearingY;

    public GlyphData(char c, uint index, byte[] bufferData, GlyphMetrics metrics, int bitmapLeft, GraphicsDevice graphics) {
      this.c = c;
      this.index = index;
      width = (int)metrics.Width;
      height = (int)metrics.Height;
      advanceX = (int)metrics.HorizontalAdvance;
      bearingX = (int)metrics.HorizontalBearingX;
      bearingY = (int)metrics.HorizontalBearingY;
      this.bitmapLeft = bitmapLeft;

      // turn buffer data into texture
      texture = new Texture2D(graphics, width, height);
      Color[] colors = new Color[width * height];
      for (int j = 0; j < height; j++) {
        for (int i = 0; i < width; i++) {
          var src = (j * width) + i;
          //var dest = (by + y) * TextureSize + bx + x;
          colors[src] = Color.FromNonPremultiplied(255, 255, 255, bufferData[src]);
        }
      }
      texture.SetData(colors);
    }
  }
}