using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Utils
{

  public partial class Node : IDisposable
  {

    public virtual bool Collides(Collider collider, Vector2 offset = new Vector2())
    {
      if (Collider == null)
      {
        return false;
      }

      return Collider.Collides(collider, offset);
    }

    public virtual bool Collides(Vector2 from, Vector2 to)
    {
      if (Collider == null)
      {
        return false;
      }

      return Collider.Collides(from, to);
    }

    /*
    TODO: circlebox
    */
    // public virtual bool Collides(Vector2 offset, CircleBox c) {
    //   //return false;
    //   if (Collider == null) {
    //     return false;
    //   }

    //   return Collider.Collides(offset, c);
    // }
  }
}