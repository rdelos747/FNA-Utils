using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Utils
{

  public partial class Sprite : Node
  {
    /*
    Spritesheets create their own internal Animation dictionary, which holds lists of frames
    keyed by a string (Id).

    When the user wants to add an animation, they use AddAnimation(name, id, ...) to tell
    the sprite which animation to use. This allows us to store mutliple animations with
    different settings that all use the same frames.

    Ex:
    The spritesheet assigned to this sprite has an animation keyd by "run-left".
    The user creates a sprite that is affected by water, and creates two animations for
    running left:
      AddAnimation("run-left", "run-left", 1) // normal speed
      AddAnimation("run-left-slow", "run-left", 2) //slower version
    */
    private class AnimationData
    {
      public string Name; // User assigned name for animation
      public string Id; // Name of the animation in the spritesheet
      public int Delay; // in milliseconds
      public bool Loop;
    }

    public Spritesheet Spritesheet { get; private set; }

    /*
    Animations
    */
    private float AnimationTime = 0;
    private int AnimationFrameIndex = 0;
    private AnimationData CurrentAnimationData = null;
    private Dictionary<string, AnimationData> Animations = new Dictionary<string, AnimationData>();
    public string CurrentAnimation { get; private set; }
    public bool Animating = false;
    public float AnimationRate = 1f;
    public Vector2? Justify;

    /*
    Frame
    */
    private string _currentFrameId;
    public string CurrentFrameId
    {
      get
      {
        return _currentFrameId;
      }
      private set
      {
        _currentFrameId = value;

        if (Justify.HasValue)
        {
          Origin = new Vector2(
            Spritesheet.Frames[CurrentFrameId].Rect.Width * Justify.Value.X,
            Spritesheet.Frames[CurrentFrameId].Rect.Height * Justify.Value.Y
          );
        }
      }
    }

    public int SpriteWidth
    {
      get
      {
        return Spritesheet.Frames[CurrentFrameId].Rect.Width;
      }
    }

    public int SpriteHeight
    {
      get
      {
        return Spritesheet.Frames[CurrentFrameId].Rect.Height;
      }
    }

    public Sprite(Spritesheet sheet)
    {
      Spritesheet = sheet;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing == true)
      {
        // Do not dispose of images or other data
        // that might be managed by the ContentManager,
        // as it keeps a cache of textures and other
        // stuff. Eg, dont do:
        // image.Dispose(); 
      }
    }

    protected override void Render()
    {
      Engine.SpriteBatch.Draw(
        Spritesheet.Texture,
        DrawPosition,
        Spritesheet.Frames[CurrentFrameId].Rect,
        Color * DrawAlpha,
        Rotation,
        Origin,
        Scale,
        SpriteEffects.None,
        Depth
      );
    }

    public override void Update()
    {
      if (!Active) return;

      /*
      Run animation
      */
      if (Animating)
      {
        AnimationTime += Engine.DeltaTime * AnimationRate;

        /*
        Frame over
        */
        if (AnimationTime >= CurrentAnimationData.Delay)
        {
          AnimationTime = 0;
          AnimationFrameIndex++;

          /*
          Animation restart/ ended
          */
          if (AnimationFrameIndex == Spritesheet.Animations[CurrentAnimationData.Id].Count)
          {
            if (CurrentAnimationData.Loop)
            {
              AnimationFrameIndex = 0;
              CurrentFrameId = Spritesheet.Animations[CurrentAnimationData.Id][AnimationFrameIndex];
              OnAnimationNewFrame(CurrentFrameId);
              OnAnimationRestart(CurrentAnimationData.Name);
            }
            else
            {
              Animating = false;
              OnAnimationEnd(CurrentAnimationData.Name);
              CurrentAnimation = null;
            }
          }
          else
          {
            CurrentFrameId = Spritesheet.Animations[CurrentAnimationData.Id][AnimationFrameIndex];
            OnAnimationNewFrame(CurrentFrameId);
          }
        }
      }

      base.Update();
    }

    public void SetFrame(string id)
    {
      if (!Spritesheet.Frames.ContainsKey(id))
      {
        throw new Exception("No Frame found in spritesheet with id: " + id);
      }

      CurrentFrameId = id;
      Animating = false;
      CurrentAnimation = null;
    }

    public void AddAnimation(string name, string id, int delay, bool loop)
    {
      if (!Spritesheet.Animations.ContainsKey(id))
      {
        throw new Exception("No Animation found in spritesheet with id: " + id);
      }

      Animations.Add(name, new AnimationData
      {
        Name = name,
        Id = id,
        Delay = delay,
        Loop = loop
      });
    }

    public void Play(string name, bool restart = false)
    {
      if (!Animations.ContainsKey(name))
      {
        throw new Exception("No Animation found with name: " + name);
      }

      if (CurrentAnimationData == null || CurrentAnimationData.Name != name || restart)
      {
        AnimationTime = 0;
        Animating = true;
        AnimationFrameIndex = 0;
        CurrentAnimationData = Animations[name];
        CurrentFrameId = Spritesheet.Animations[CurrentAnimationData.Id][0];
        CurrentAnimation = name;
        OnAnimationStart(CurrentAnimation);
      }
    }

    public virtual void OnAnimationStart(string name) { }

    public virtual void OnAnimationEnd(string name) { }

    public virtual void OnAnimationRestart(string name) { }

    public virtual void OnAnimationNewFrame(string name) { }
  }
}