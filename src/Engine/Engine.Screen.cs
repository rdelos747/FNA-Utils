using System;
using System.IO;
using System.Runtime;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Utils {

  public partial class Engine : Game {
    protected virtual void OnGraphicsReset(object sender, EventArgs e) {
      UpdateView();

      // if (scene != null)
      //   scene.HandleGraphicsReset();
      // if (nextScene != null && nextScene != scene)
      //   nextScene.HandleGraphicsReset();
    }

    protected virtual void OnGraphicsCreate(object sender, EventArgs e) {
      UpdateView();

      // if (scene != null)
      //   scene.HandleGraphicsCreate();
      // if (nextScene != null && nextScene != scene)
      //   nextScene.HandleGraphicsCreate();
    }

    private void UpdateView() {
      float screenWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
      float screenHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;

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
      ViewWidth -= ViewPadding * 2;
      ViewHeight -= (int)(aspect * ViewPadding * 2);

      // update viewport
      Viewport = new Viewport {
        X = (int)(screenWidth / 2 - ViewWidth / 2),
        Y = (int)(screenHeight / 2 - ViewHeight / 2),
        Width = ViewWidth,
        Height = ViewHeight,
        MinDepth = 0,
        MaxDepth = 1
      };

      foreach (Renderer renderer in Renderers) {
        renderer.UpdateView(GraphicsDevice);
      }
    }
  }
}