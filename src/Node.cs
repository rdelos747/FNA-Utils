using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Engine {

  /*
  Basic unit that the renderer stores and loops over for drawing
  */

  public class Node : IDisposable {

    private Node Parent;
    private List<Node> Nodes = new List<Node>();
    private bool Disposed = false;

    // Position stuff
    public float X;
    public float Y;

    // Origin Stuff
    private Vector2 _origin = new Vector2();
    public Vector2 Origin {
      get {
        return _origin;
      }
      set {
        _origin = value;
        _bounds = new Rectangle((int)-_origin.X, (int)-_origin.Y, _bounds.Width, _bounds.Height);
      }
    }

    // Bounds stuff
    private Rectangle _bounds = new Rectangle();
    public Rectangle Bounds {
      get {
        return _bounds;
      }
      set {
        _bounds = value;
        _origin = new Vector2(-_bounds.X, -_bounds.Y);
      }
    }
    public bool ShowBounds = false;
    public Color BoundsColor = Color.Blue;
    public float BoundsAlpha = 0.5f;

    public Node() { }

    // public Node(ContentManager content) {
    //   // This is mainly used to inject the content
    //   // manager into the root node.
    //   this.Content = content;
    // }

    public void addChild(Node n) {
      if (n.Parent == null) {
        Nodes.Add(n);
        n.Parent = this;
        // n.Content = Content;
        // n.load(Content);
      }
    }

    public void removeFromParent() {
      if (Parent != null) {

        // if we have children, go down that path first
        for (int i = 0; i < Nodes.Count; i++) {
          Nodes[i].removeFromParent();
          Nodes.Remove(Nodes[i]);
        }

        // then, handle ourselves
        Parent.Nodes.Remove(this);
        Parent = null;
        Dispose();
      }
    }

    //public virtual void load(ContentManager content) { }

    public virtual void draw(SpriteBatch spriteBatch, float lastX, float lastY) {
      float worldX = lastX + X;
      float worldY = lastY + Y;

      if (!_bounds.IsEmpty && ShowBounds) {
        spriteBatch.Draw(
          Renderer.systemRect,
          new Rectangle((int)(worldX + _bounds.X), (int)(worldY + _bounds.Y), _bounds.Width, _bounds.Height),
          BoundsColor * BoundsAlpha
        );
      }
      for (int i = 0; i < Nodes.Count; i++) {
        Node n = Nodes[i];
        n.draw(spriteBatch, worldX, worldY);
      }
    }

    // public Vector2 getPositionInParent() {
    //   return new Vector2();
    // }


    public void Dispose() {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) { }

    ~Node() {
      Dispose(false);
    }
  }
}