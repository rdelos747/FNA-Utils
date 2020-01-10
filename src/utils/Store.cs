using System;

namespace Engine {

  public delegate void Dispatch(string action, Object payload = null);

  public delegate T Reducer<T>(T state, String action, Object payload);

  public sealed class Store<T> {
    private Reducer<T> reducer;
    public T state { get; private set; }

    public Store(Reducer<T> reducer, T initialState) {
      this.reducer = reducer;
      state = initialState;
    }

    public void dispatch(string action, Object payload) {
      Console.WriteLine("hererere");
      state = reducer(state, action, payload);
    }
  }
}