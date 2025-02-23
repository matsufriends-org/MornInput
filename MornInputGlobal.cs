using MornGlobal;
using UnityEngine;

namespace MornInput
{
    [CreateAssetMenu(fileName = nameof(MornInputGlobal), menuName = "Morn/" + nameof(MornInputGlobal))]
    internal sealed class MornInputGlobal : MornGlobalBase<MornInputGlobal>
    {
        protected override string ModuleName => nameof(MornInput);
        [SerializeField] private string _defaultSchemeKey;
        public string DefaultSchemeKey => _defaultSchemeKey;
        
        public static void Log(string message)
        {
            I.LogInternal(message);
        }

        public static void LogError(string message)
        {
            I.LogErrorInternal(message);
        }

        public static void LogWarning(string message)
        {
            I.LogWarningInternal(message);
        }
        
        public static void SetDirty(Object obj)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(obj);
#endif
        }
    }
}