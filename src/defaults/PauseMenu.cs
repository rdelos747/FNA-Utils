using System;
using Microsoft.Xna.Framework;

namespace Engine {
  public static class PauseMenuPage {
    public const string Home = "Home";
    public const string Settings = "Settings";
    public const string Controls = "Controls";
  }

  public sealed class PauseMenu : Element {
    private Renderer Renderer;

    private Element CurrentPage;
    private Element Title;
    private Element SubTitle;

    const int MENU_LEFT = 50;
    const int MENU_TOP = 200;
    const int TITLE_MARGIN = EngineDefaults.fontSizeLarge;
    const int SUB_TITLE_MARGIN = 75;

    public PauseMenu(Renderer renderer) {
      Renderer = renderer;
      Renderer.AddChildToRoot(this);

      Bounds = new Rectangle(0, 0, Parent.Bounds.Width, Parent.Bounds.Height);
      ShowBounds = true;
      BoundsColor = EngineDefaults.PauseMenuBackground;
      BoundsAlpha = EngineDefaults.PauseMenuAlpha;

      LeftOffset = MENU_LEFT;
      TopOffset = MENU_TOP;

      Title = new Element();
      Title.Label.Font = EngineDefaults.SystemFontLarge;
      Title.Label.Text = "Menu";
      AddChildAsElement(Title, 0, 0);

      SubTitle = new Element();
      SubTitle.Label.Text = PauseMenuPage.Home;
      AddChildAsElement(SubTitle, 0, TITLE_MARGIN);

      TopOffset += SUB_TITLE_MARGIN;

      SetPage(PauseMenuPage.Home);

      OnOpenMenu();
    }

    public void SetPage(String page) {
      if (CurrentPage != null) {
        CurrentPage.RemoveFromParent();
      }

      SubTitle.Label.Text = page;

      if (page == PauseMenuPage.Home) {
        CurrentPage = new PauseMenuHome(SetPage, CloseMenu, ExitGame);
        AddChildAsElement(CurrentPage, 0, 0);
      }
      else if (page == PauseMenuPage.Settings) {
        CurrentPage = new PauseMenuSettings(SetPage);
        AddChildAsElement(CurrentPage, 0, 0);
      }
      else if (page == PauseMenuPage.Controls) {
        CurrentPage = new PauseMenuControls(SetPage);
        AddChildAsElement(CurrentPage, 0, 0);
      }
    }

    private void OnOpenMenu() {
      Renderer.IsMouseVisible = true;
    }

    private void CloseMenu() {
      Renderer.IsMouseVisible = false;
      Renderer.engineState = EngineState.RUNNING;
      RemoveFromParent();
    }

    private void ExitGame() {
      Renderer.engineState = EngineState.QUIT;
    }
  }


  public sealed class PauseMenuHome : Element {
    //private Renderer Renderer;

    const int BTN_MARGIN = 50;

    private Action CloseMenu;

    public PauseMenuHome(Action<String> setPage, Action closeMenu, Action exitGame) {
      CloseMenu = closeMenu;

      Button resumeButton = new Button();
      resumeButton.OnClick = () => { closeMenu(); };
      resumeButton.Label.Text = "Resume Game";
      AddChildAsElement(resumeButton, 0, 0);

      Button settingsButton = new Button();
      settingsButton.OnClick = () => { setPage(PauseMenuPage.Settings); };
      settingsButton.Label.Text = "Settings";
      AddChildAsElement(settingsButton, 0, BTN_MARGIN);

      Button aboutButton = new Button();
      aboutButton.Label.Text = "About";
      AddChildAsElement(aboutButton, 0, BTN_MARGIN);

      Button exitButton = new Button();
      exitButton.OnClick = () => { exitGame(); };
      exitButton.Label.Text = "Exit Game";
      AddChildAsElement(exitButton, 0, BTN_MARGIN);
    }

    public override void Update(float mouseX, float mouseY) {
      if (Input.keyPressed(EngineDefaults.keySecondary)) {
        CloseMenu();
      }

      base.Update(mouseX, mouseY);
    }
  }

  public sealed class PauseMenuSettings : Element {
    const int BTN_MARGIN = 50;
    private Action<string> SetPage;

    public PauseMenuSettings(Action<String> setPage) {
      SetPage = setPage;

      Button controlsButton = new Button();
      controlsButton.OnClick = () => { setPage(PauseMenuPage.Controls); };
      controlsButton.Label.Text = "Controls";
      AddChildAsElement(controlsButton, 0, 0);

      Button cancelButton = new Button();
      cancelButton.OnClick = () => { setPage(PauseMenuPage.Home); };
      cancelButton.Label.Text = "Cancel";
      AddChildAsElement(cancelButton, 0, BTN_MARGIN);
    }

    public override void Update(float mouseX, float mouseY) {
      if (Input.keyPressed(EngineDefaults.keySecondary)) {
        SetPage(PauseMenuPage.Home);
      }

      base.Update(mouseX, mouseY);
    }
  }

  public sealed class PauseMenuControls : Element {
    const int BTN_MARGIN = 50;
    private Action<string> SetPage;

    public PauseMenuControls(Action<String> setPage) {
      SetPage = setPage;

      // Button controlsButton = new Button();
      // controlsButton.OnClick = () => { setPage(PauseMenuPage.Home); };
      // controlsButton.Label.Text = "Controls";
      // AddChildAsElement(controlsButton, 0, 0);

      Button cancelButton = new Button();
      cancelButton.OnClick = () => { setPage(PauseMenuPage.Settings); };
      cancelButton.Label.Text = "Cancel";
      AddChildAsElement(cancelButton, 0, BTN_MARGIN);
    }

    public override void Update(float mouseX, float mouseY) {
      if (Input.keyPressed(EngineDefaults.keySecondary)) {
        SetPage(PauseMenuPage.Settings);
      }

      base.Update(mouseX, mouseY);
    }
  }
}
