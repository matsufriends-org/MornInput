using UnityEditor;
using UnityEngine;

namespace MornInput
{
    internal static class MornInputUtil
    {
#if DISABLE_MORN_ARBOR_LOG
        private const bool ShowLOG = false;
#else
        private const bool ShowLOG = true;
#endif
        private const string Prefix = "[<color=green>MornInput</color>] ";

        internal static void Log(string message)
        {
            if (ShowLOG) Debug.Log(Prefix + message);
        }

        internal static void LogError(string message)
        {
            if (ShowLOG) Debug.LogError(Prefix + message);
        }

        internal static void LogWarning(string message)
        {
            if (ShowLOG) Debug.LogWarning(Prefix + message);
        }

        internal static void SetDirty(Object obj)
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(obj);
#endif
        }
    }
}