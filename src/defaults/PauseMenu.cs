using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Engine {
  public static class PauseMenuPage {
    public const string Home = "Home";
    public const string Settings = "Settings";
    public const string Controls = "Controls";
  }

  public sealed class PauseMenu : Element {
    private Renderer Renderer;

    private Font FontLarge;
    private Font FontReg;

    private string CurrentPageName = PauseMenuPage.Home;

    private Element CurrentPage;
    private Element Title;
    private Element SubTitle;

    private int TITLE_Y_MARGIN = EngineDefaults.FontSizeLarge;
    private int SUB_TITLE_Y_MARGIN = 75;

    private Dictionary<string, int> IndexStore = new Dictionary<string, int>() {
      {PauseMenuPage.Home, 0},
      {PauseMenuPage.Settings, 0},
      {PauseMenuPage.Controls, 0}
    };

    public PauseMenu(Renderer renderer, FontLib fontLib = null) {
      Renderer = renderer;
      Renderer.AddChildToRoot(this);

      if (fontLib != null) {
        FontReg = fontLib.CreateFont(EngineDefaults.FontSizeReg);
        FontLarge = fontLib.CreateFont(EngineDefaults.FontSizeLarge);
      }
      else {
        FontReg = Renderer.SystemFontLib.CreateFont(EngineDefaults.FontSizeReg);
        FontLarge = Renderer.SystemFontLib.CreateFont(EngineDefaults.FontSizeLarge);
      }

      Bounds = new Rectangle(0, 0, Parent.Bounds.Width, Parent.Bounds.Height);
      ShowBounds = true;
      BoundsColor = EngineDefaults.PauseMenuBackground;
      BoundsAlpha = EngineDefaults.PauseMenuAlpha;

      X = EngineDefaults.MenuLeft;
      Y = EngineDefaults.MenuTop;

      Label.Font = FontLarge;
      Label.Text = "Menu";

      SubTitle = new Element(FontReg);
      SubTitle.Label.Text = PauseMenuPage.Home;
      AddChildAsElement(SubTitle, 0, TITLE_Y_MARGIN);

      TopOffset += SUB_TITLE_Y_MARGIN;

      SetPage(PauseMenuPage.Home);

      OnOpenMenu();
    }

    public void SetPage(String page) {
      if (CurrentPage != null) {
        IndexStore[CurrentPageName] = CurrentPage.CurrentSelectedChildIndex;
        CurrentPage.RemoveFromParent();
      }

      CurrentPageName = page;
      SubTitle.Label.Text = page;

      int idx = IndexStore[page];
      if (page == PauseMenuPage.Home) {
        CurrentPage = new PauseMenuHome(idx, SetPage, CloseMenu, ExitGame);
        AddChildAsElement(CurrentPage, 0, 0);
      }
      else if (page == PauseMenuPage.Settings) {
        CurrentPage = new PauseMenuSettings(idx, SetPage);
        AddChildAsElement(CurrentPage, 0, 0);
      }
      else if (page == PauseMenuPage.Controls) {
        CurrentPage = new PauseMenuControls(idx, SetPage);
        AddChildAsElement(CurrentPage, 0, 0);
      }
    }

    private void OnOpenMenu() {
      Renderer.IsMouseVisible = true;
    }

    private void CloseMenu() {
      Renderer.IsMouseVisible = false;
      Renderer.EngineState = EngineStates.RUNNING;
      RemoveFromParent();
    }

    private void ExitGame() {
      Renderer.EngineState = EngineStates.QUIT;
    }
  }


  public sealed class PauseMenuHome : Element {
    const int BTN_MARGIN = 50;

    private Action CloseMenu;

    public PauseMenuHome(int startIndex, Action<String> setPage, Action closeMenu, Action exitGame) {
      CurrentSelectedChildIndex = startIndex;
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
      if (Input.KeyPressed(EngineDefaults.KeySecondary)) {
        CloseMenu();
      }

      base.Update(mouseX, mouseY);
    }
  }

  public sealed class PauseMenuSettings : Element {
    const int BTN_MARGIN = 50;
    private Action<string> SetPage;

    public PauseMenuSettings(int startIndex, Action<String> setPage) {
      CurrentSelectedChildIndex = startIndex;

      SetPage = setPage;

      Button testButton = new Button();
      testButton.OnClick = () => { setPage(PauseMenuPage.Controls); };
      testButton.Label.Text = "test";
      AddChildAsElement(testButton, 0, 0);

      Button controlsButton = new Button();
      controlsButton.OnClick = () => { setPage(PauseMenuPage.Controls); };
      controlsButton.Label.Text = "Controls";
      AddChildAsElement(controlsButton, 0, BTN_MARGIN);

      Button cancelButton = new Button();
      cancelButton.OnClick = () => { setPage(PauseMenuPage.Home); };
      cancelButton.Label.Text = "Cancel";
      AddChildAsElement(cancelButton, 0, BTN_MARGIN);
    }

    public override void Update(float mouseX, float mouseY) {
      if (Input.KeyPressed(EngineDefaults.KeySecondary)) {
        SetPage(PauseMenuPage.Home);
      }

      base.Update(mouseX, mouseY);
    }
  }

  public sealed class PauseMenuControls : Element {
    const int BTN_MARGIN = 50;
    private Action<string> SetPage;
    private List<KeySwitcher> KeySwitchers;

    public PauseMenuControls(int startIndex, Action<String> setPage) {
      CurrentSelectedChildIndex = startIndex;

      SetPage = setPage;



      // KeySwitcher test = new KeySwitcher(EngineDefaults.keyPrimary);
      // AddChildAsElement(test, 0, 0);

      List list = new List(1);
      AddChildAsElement(list, 0, 0);

      Button cancelButton = new Button();
      cancelButton.OnClick = () => { setPage(PauseMenuPage.Settings); };
      cancelButton.Label.Text = "Cancel";
      list.AddChildAsElement(cancelButton, 0, 0);

      foreach (string key in Input.InputMap.Keys) {
        KeySwitcher k = new KeySwitcher(key);

        list.AddChildAsElement(k, 0, BTN_MARGIN);
      }
    }

    public override void Update(float mouseX, float mouseY) {
      if (Input.KeyPressed(EngineDefaults.KeySecondary)) {
        SetPage(PauseMenuPage.Settings);
      }

      base.Update(mouseX, mouseY);
    }
  }
}
