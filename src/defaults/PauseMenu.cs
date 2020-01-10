namespace Engine {
  public sealed class PauseMenu : Menu {
    public PauseMenu(Renderer p) : base(p, 0, 0, 100, 100) { }

    public override void init() {
      addElement(new Element(this));
      base.init();
    }
  }
}