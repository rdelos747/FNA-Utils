using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Engine {

  // public static class SystemRect {
  //   public static Texture2D Texture;


  // }

  /*
  Basic unit that the renderer stores and loops over for drawing
  */

  public class Node : IDisposable {

    private bool Disposed = false;

    // child node stuff
    private List<Node> _nodes = new List<Node>();
    public List<Node> Nodes {
      get {
        return _nodes;
      }
      private set {
        _nodes = value;
      }
    }

    public Node Parent { get; private set; }


    // Position stuff
    public float X;
    public float Y;

    // Bounds stuff
    public static Texture2D BoundsTexture;

    private Rectangle _bounds = new Rectangle();
    public Rectangle Bounds {
      get {
        return _bounds;
      }
      set {
        _bounds = value;
        //_origin = new Vector2(-(_bounds.X * 2), -(_bounds.Y * 2));
      }
    }
    public bool ShowBounds = false;
    public Color BoundsColor = Color.Blue;
    public float BoundsAlpha = 0.5f;
    public bool IsHidden = false;

    public bool ShowCenter = false;

    public Node() { }

    public void AddChild(Node n) {
      if (n.Parent == null) {
        _nodes.Add(n);
        n.Parent = this;
      }
    }

    public void RemoveFromParent(bool isMe = true) {
      if (Parent != null) {
        foreach (Node n in _nodes) {
          n.RemoveFromParent(false);
        }
        // then, handle ourselves
        _nodes.Clear();

        if (isMe) {
          Parent._nodes.Remove(this);
        }

        Parent = null;
        Dispose();
      }
    }

    public virtual void Draw(SpriteBatch spriteBatch, float lastX, float lastY) {
      if (IsHidden) return;

      float relativeX = lastX + X;
      float relativeY = lastY + Y;

      if (!_bounds.IsEmpty && ShowBounds) {
        spriteBatch.Draw(
          BoundsTexture,
          new Rectangle((int)(relativeX + _bounds.X), (int)(relativeY + _bounds.Y), _bounds.Width, _bounds.Height),
        BoundsColor * BoundsAlpha
        );
      }

      // for position debugging
      if (ShowCenter) {
        spriteBatch.Draw(
          BoundsTexture,
          new Rectangle((int)(relativeX), (int)(relativeY), 2, 2),
          Color.Red
        );
      }

      for (int i = 0; i < _nodes.Count; i++) {
        Node n = _nodes[i];
        n.Draw(spriteBatch, relativeX, relativeY);
      }
    }

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