// using System;
// using System.Collections.Generic;
// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Graphics;
// using Microsoft.Xna.Framework.Content;



// namespace Engine {
//   // public sealed class Button : GameObject {
//   //   public string message;
//   //   public bool selected = false;

//   //   public Button(string newMessage, int x = 0, int y = 0, int w = 10, int h = 10) {
//   //     bounds = new Rectangle(x, y, w, h);
//   //     showBounds = true;
//   //   }

//   //   public override void update() {
//   //     if (Input.mouseLeftClicked() && pointInBounds(Input.mouseX, Input.mouseY)) {
//   //       return message;
//   //     }

//   //     if (Input.keyPressed(EngineDefaults.keyPrimary) && selected) {
//   //       return message;
//   //     }

//   //     return null;
//   //   }

//   //   // public void draw(SpriteBatch spriteBatch, GameTime gameTime) {
//   //   //   if (bounds != null) {
//   //   //     spriteBatch.Draw(Renderer.systemRect, bounds, bkColor * bkAlpha);
//   //   //   }
//   //   // }
//   // }

//   public class Button : Element {
//     public Button(Menu p, int x = 0, int y = 0, int w = 0, int h = 0) : base(p, x, y, w, h) {

//     }
//   }
// }