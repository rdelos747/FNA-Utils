using System;

namespace Utils {

  public static class EnumHelper {

    public static T NextFrom<T>(T src) {
      if (!typeof(T).IsEnum) {
        throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));
      }

      T[] arr = (T[])Enum.GetValues(typeof(T));
      int j = Array.IndexOf<T>(arr, src) + 1;
      return (arr.Length == j) ? arr[0] : arr[j];
    }

    public static T PrevFrom<T>(T src) {
      if (!typeof(T).IsEnum) {
        throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));
      }

      T[] arr = (T[])Enum.GetValues(typeof(T));
      int j = Array.IndexOf<T>(arr, src) - 1;
      return (j == -1) ? arr[arr.Length - 1] : arr[j];
    }
  }
}