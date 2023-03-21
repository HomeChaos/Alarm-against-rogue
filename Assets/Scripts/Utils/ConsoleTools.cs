using UnityEngine;

namespace Scripts.Utils
{
    static public class ConsoleTools
    {
        static public void LogSuccess(string message, int size = 12)
        {
            Debug.Log($"<size={GetMaxSize(size)}><color=YELLOW>{message}</color></size>");
        }

        static public void LogInfo(string message, int size = 12)
        {
            Debug.Log($"<size={GetMaxSize(size)}><color=ORANGE>{message}</color></size>");
        }

        static public void LogError(string message, int size = 12)
        {
            Debug.Log($"<b><size={GetMaxSize(size)}><color=RED>{message}</color></size></b>");
        }

        static private int GetMaxSize(int size) => Mathf.Min(size, 20);
    }
}