using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Engine {
  public class Element : GameObject {
    private Menu parent;

    public string message = "";
    public bool selected = false;

    public Object action;

    public Element(Menu p, int x = 0, int y = 0, int w = 50, int h = 50) { // parent here should really be the redux store
      parent = p;
      bounds = new Rectangle(x, y, w, h);
      showBounds = true;
    }

    public virtual void update() {
      if (Input.mouseLeftClicked() && pointInBounds(Input.mouseX, Input.mouseY)) {
        parent.handleElementClick(message);
      }

      if (Input.keyPressed(EngineDefaults.keyPrimary) && selected) {
        parent.handleElementClick(message);
      }
    }
  }
}
