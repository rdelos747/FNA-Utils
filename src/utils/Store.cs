using System;

namespace Utils
{
  public class Store
  {
    public delegate void SubscribeDelegate();
    private event SubscribeDelegate Subscribe;

    public void AddSubscriber(SubscribeDelegate sub)
    {
      Subscribe += sub;
    }

    public void RemoveSubscriber(SubscribeDelegate sub)
    {
      //Console.WriteLine("....Removing Subscriber");
      Subscribe -= sub;
    }

    public int? NumSubscribers()
    {
      return Subscribe?.GetInvocationList().Length;
    }

    public void Dispatch(Action action)
    {
      action();

      Subscribe?.Invoke();
    }

    public void Dispatch()
    {
      Subscribe?.Invoke();
    }
  }
}