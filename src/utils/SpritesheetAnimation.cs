using System.Collections.Generic;

namespace Engine {
  /*
  Animation class used to iterate through a list of frames (just ints).
   */

  public class SpritesheetAnimation {
    public List<int> frames;
    public int delay;
    public int frameIndex;
    public bool paused = false;
    public bool repeat = true;

    private int currentTime = 0;

    public SpritesheetAnimation(int[] newFrames, int startIndex = 0, int newDelay = 0) {
      frames = new List<int>(newFrames);
      frameIndex = startIndex;
      delay = newDelay;
    }

    public int getFrame() {
      return frames[frameIndex];
    }

    public void run() {
      if (paused || frames.Count == 0) return;

      if (++currentTime < delay) return;

      currentTime = 0;
      ++frameIndex;

      if (frameIndex == frames.Count) {
        if (repeat) {
          frameIndex = 0;
        }
        else {
          frameIndex = frameIndex - 1;
          paused = true;
        }
      }
    }
  }
}