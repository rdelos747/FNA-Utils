using System.Collections.Generic;

namespace Utils
{

  public class Scene
  {
    public List<Renderer> Renderers = new List<Renderer>();
    public bool Active = true;
    public bool Visible = true;

    public virtual void Update()
    {
      foreach (Renderer renderer in Renderers)
      {
        renderer.Update();
      }
    }

    public virtual void Draw()
    {
      foreach (Renderer renderer in Renderers)
      {
        Engine.CurrentRenderer = renderer;
        renderer.Draw();
      }
    }

    protected void Add(Renderer r)
    {
      Renderers.Add(r);
    }
  }
}