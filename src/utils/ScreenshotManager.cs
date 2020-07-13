using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Utils {

  public class ScreenshotManager {
    public string DirName;
    public string Prefix;
    public GraphicsDevice Graphics;

    public ScreenshotManager(GraphicsDevice graphics, string dirName, string prefix) {
      Graphics = graphics;
      DirName = dirName;
      Prefix = prefix + "_";
    }

    public void Capture(string info = "") {
      /*
      Load screenshot directory,
      */
      DirectoryInfo dir = new DirectoryInfo(DirName);
      if (!dir.Exists) {
        Directory.CreateDirectory(DirName);
      }

      /*
      Render screen to texture
      */
      int w = Graphics.PresentationParameters.BackBufferWidth;
      int h = Graphics.PresentationParameters.BackBufferHeight;

      //Draw(new GameTime());

      int[] backBuffer = new int[w * h];
      Graphics.GetBackBufferData(backBuffer);

      Texture2D texture = new Texture2D(Graphics, w, h, false, Graphics.PresentationParameters.BackBufferFormat);
      texture.SetData(backBuffer);

      /*
      Save png
      */
      string extraInfo = "";
      if (info != "") {
        extraInfo = $"{info}_";
      }
      string scName = DirName + "/" + extraInfo + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".png";
      Console.WriteLine($"SAVING SCREENSHOT {scName}");
      Stream stream = File.OpenWrite(scName);

      texture.SaveAsPng(stream, w, h);
      stream.Dispose();
      texture.Dispose();
    }
  }
}