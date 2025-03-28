using System;
using UnityEngine;

namespace MornInput
{
    [CreateAssetMenu(fileName = nameof(MornPressableInputIconSettings), menuName = "Morn/" + nameof(MornPressableInputIconSettings))]
    public class MornPressableInputIconSettings : ScriptableObject
    {
        public Vector2 NormalOffset;
        public Vector2 PressedOffset;

        public Color NormalColor;
        public Color PressedColor;
    }
}