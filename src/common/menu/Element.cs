using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Engine {

  public class Element : GameObject {
    /*
      An element can be selected. When an element gets added to a parent element,
      if it IsSelectable, the added element gets assigned an index and the 
      parent's NumSelectableChildren increments.
    */

    public bool Selected = false;
    public bool IsSelectable = false;
    private int SelectIndex = -1;

    private int NumSelectableChildren = 0;
    public int CurrentSelectedChildIndex { get; protected set; } = 0;

    public int TopOffset = 0;
    public int LeftOffset = 0;

    public Color BackgroundColor = EngineDefaults.ButtonBackgroundColor;
    public float BackgroundAlpha = EngineDefaults.ButtonBackgroundAlpha;
    public Color SelectedColor = EngineDefaults.ButtonSelectedColor;
    public float SelectedAlpha = EngineDefaults.ButtonSelectedAlpha;
    public Color TextColor = EngineDefaults.ButtonTextColor;
    public Color TextSelectedColor = EngineDefaults.ButtonTextSelectedColor;

    public TextObject Label;

    public Element(Font font = null) {
      BoundsAlpha = 1f;

      Label = new TextObject(font);
      Label.VerticalAlignment = VerticalAlignment.CENTER;

      AddChild(Label);
    }

    public virtual void Update(float mouseX, float mouseY) {
      // handle selection by mouse click
      if (IsSelectable && Input.MouseLeftClicked() && pointInBounds(mouseX, mouseY)) {
        Element parent = Parent as Element;
        if (parent != null) {
          parent.UnselectAllChildren();
          parent.CurrentSelectedChildIndex = SelectIndex;
        }
      }

      // handle child selection index by arrow keys
      if (NumSelectableChildren > 0) {
        if (Input.KeyPressed(EngineDefaults.KeyDown)) {
          CurrentSelectedChildIndex++;
          if (CurrentSelectedChildIndex >= NumSelectableChildren) {
            CurrentSelectedChildIndex = 0;
          }
        }

        if (Input.KeyPressed(EngineDefaults.KeyUp)) {
          CurrentSelectedChildIndex--;
          if (CurrentSelectedChildIndex <= -1) {
            CurrentSelectedChildIndex = NumSelectableChildren - 1;
          }
        }
      }

      // update children with current selection index and
      // pass mouse offset 
      float relativeX = mouseX - X;
      float relativeY = mouseY - Y;
      for (int i = 0; i < Nodes.Count; i++) {
        Element el = Nodes[i] as Element;
        if (el != null) {
          el.Update(relativeX, relativeY);

          if (el.IsSelectable) {
            el.SetUnselected();
            if (CurrentSelectedChildIndex == el.SelectIndex) {
              el.SetSelected();
            }
            if (el.pointInBounds(relativeX, relativeY)) {
              el.SetSelected();
            }
          }

        }
      }
    }

    public virtual void SetSelected() { }
    public virtual void SetUnselected() { }

    private void UnselectAllChildren() {
      for (int i = 0; i < Nodes.Count; i++) {
        Element el = Nodes[i] as Element;
        if (el != null) {
          el.SetUnselected();
        }
      }
    }


    /*
      To assist with adding multiple child elements at specific offests, the parent
      element holds CurrentChildTopOffset and CurrentChildLeftOffset. When adding
      child elements, we only need to worry about specifying the current offset from
      the last element added.
    */
    public void AddChildAsElement(Element el, int left, int top) {
      AddChild(el);
      LeftOffset += left;
      TopOffset += top;
      el.X = LeftOffset;
      el.Y = TopOffset;

      if (el.IsSelectable) {
        el.SelectIndex = NumSelectableChildren;
        NumSelectableChildren++;
      }
    }
  }
}
