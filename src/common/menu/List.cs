using System;

namespace Utils {

  public sealed class List : Element {

    private int Rows = 0;
    private int CurrentTopRow = 0;
    private int MaxRows;
    private int RowHeight;

    public List(int maxRows) {
      MaxRows = maxRows;
    }

    //public override void AddChildAsElement(Element el, int left, int top) {}

    new public void AddChildAsElement(Element el, int top, int left) {
      base.AddChildAsElement(el, top, left);
      Rows++;
      if (Rows > MaxRows) {
        el.IsHidden = true;
      }
    }

    // Rows 5
    // MaxRows 1
    // 

    public override void Update(float mouseX, float mouseY) {
      base.Update(mouseX, mouseY);

      if (CurrentSelectedChildIndex >= MaxRows && CurrentSelectedChildIndex - MaxRows >= CurrentTopRow) {
        CurrentTopRow++;
        Console.WriteLine("went up, " + CurrentSelectedChildIndex);

        for (int i = 0; i < Nodes.Count; i++) {
          if (i >= CurrentTopRow && i < CurrentTopRow + MaxRows) {
            Nodes[i].IsHidden = false;
          }
          else {
            Nodes[i].IsHidden = true;
          }
        }
      }
      //else if ()
    }
  }
}