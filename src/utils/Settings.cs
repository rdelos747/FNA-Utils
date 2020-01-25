using System;

namespace Engine {

  public delegate void Dispatch(string action, Object payload = null);

  public abstract class Settings {
    public Settings() { }

    public void dispatch(string action, Object payload) {
      reducer(action, payload);
    }

    protected virtual void reducer(string action, Object payload) { }
  }
}