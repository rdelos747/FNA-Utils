using System;
using System.Collections.Generic;

namespace Utils
{

  public class Scene
  {
    public List<Renderer> Renderers = new List<Renderer>();
    public bool Active = true;
    public bool Visible = true;

    public virtual void BeforeUpdate() { }

    public virtual void Update()
    {
      for (int i = 0; i < Renderers.Count; i++)
      {
        Renderers[i].Update();
      }
    }

    public virtual void AfterUpdate() { }

    public virtual void Draw()
    {
      foreach (Renderer renderer in Renderers)
      {
        Engine.CurrentRenderer = renderer;
        renderer.Draw();
      }
    }

    public void Add(Renderer r)
    {
      Renderers.Add(r);
      r.Scene = this;
    }

    public virtual void Start() { }

    public virtual void End()
    {
      for (int i = Renderers.Count - 1; i >= 0; i--)
      {
        Renderers[i].RemoveFromScene();
      }
      Console.WriteLine("Ended scene");
    }
  }
}