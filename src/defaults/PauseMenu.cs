namespace Engine {
  public sealed class PauseMenu : Menu {
    public PauseMenu() : base(0, 0, 100, 100) {

    }
  }
}

// namespace Engine {
//   public sealed class PauseMenu : Menu {
//     public PauseMenu(Renderer p) : base(p, 0, 0, 100, 100) { }

//     public override void init() {
//       Element e = new Element(this);
//       e.message = "update_thing";
//       addElement(e);

//       base.init();

//       dispatch(SettingTypes.MENU_OPEN);
//     }

//     public override void close() {
//       dispatch(SettingTypes.MENU_CLOSE);
//       base.close();
//     }
//   }
// }