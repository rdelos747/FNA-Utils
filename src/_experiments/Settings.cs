/*
  usage:

  essentially tried to impliment the concept of state mgmt

  It worked fine, but it might be a bit overkill for this kind of engine.
  For example, many classes contain references to the Renderer/ some other
  parent object. It is easier and more readable to do 
  
  Parent.Thing = something

  than it is to create and call dispatches on the store. 
*/

// using System;

// namespace Engine {

//   public delegate void Dispatch(string action, Object payload = null);

//   public abstract class Settings {
//     public Settings() { }

//     public void dispatch(string action, Object payload) {
//       reducer(action, payload);
//     }

//     protected virtual void reducer(string action, Object payload) { }
//   }
// }