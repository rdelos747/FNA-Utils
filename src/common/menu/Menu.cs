using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Engine {

  public class Menu : GameObject {

    private List<Element> elements;

    public Menu(int x, int y, int w, int h) {
      Bounds = new Rectangle(x, y, w, h);
      ShowBounds = true;
      elements = new List<Element>();
    }

    public void addElement(Element e) {
      elements.Add(e);
      addChild(e);
    }
  }
}

// namespace Engine {
//   public class Menu : GameObject {

//     //private Renderer parent;
//     private List<Element> elements = new List<Element>();

//     public Dispatch dispatch;

//     private int index = 0;

//     public Menu(/*Renderer p,*/ int x, int y, int w, int h) {
//       //parent = p;
//       bounds = new Rectangle(x, y, w, h);
//       showBounds = true;
//     }

//     public void addElement(Element el) {
//       elements.Add(el);
//       //parent.addObject(el);
//       addChild(el);
//     }

//     // public void removeElement(Element el) {
//     //   Element elToRemove = elements.SingleOrDefault(e => e == el);
//     //   if (elToRemove == null) {
//     //     throw new System.ArgumentException("Cannot remove element from menu - element not held by menu");
//     //   }
//     //   el.removeFromRenderer();
//     //   elements.Remove(elToRemove);
//     // }

//     // public virtual void init() {
//     //   parent.addObject(this);
//     // }

//     public virtual void close() {
//       for (int i = 0; i < elements.Count; i++) {
//         elements[i].removeFromRenderer();
//       }
//       elements.Clear();
//       removeFromRenderer();
//     }

//     public virtual void update() {
//       handleElementSelection();

//       for (int i = 0; i < elements.Count; i++) {
//         elements[i].update();
//       }
//     }

//     public virtual void handleElementClick(string key, Object val = null) {
//       if (dispatch != null) {
//         dispatch(key, val);
//       }
//     }

//     private void handleElementSelection() {
//       elements[index].selected = false;

//       if (Input.keyPressed(EngineDefaults.keyDown)) {
//         index++;
//         if (index == elements.Count) {
//           index = 0;
//         }
//       }

//       if (Input.keyPressed(EngineDefaults.keyUp)) {
//         index--;
//         if (index == -1) {
//           index = elements.Count - 1;
//         }
//       }

//       elements[index].selected = true;
//     }
//   }
// }
