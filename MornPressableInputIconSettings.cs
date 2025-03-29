using System;
using UnityEngine;

namespace MornInput
{
    [CreateAssetMenu(fileName = nameof(MornPressableInputIconSettings), menuName = "Morn/" + nameof(MornPressableInputIconSettings))]
    public class MornPressableInputIconSettings : ScriptableObject
    {
        public Color TopColor;
        public Color TopPressedColor;
        public Color BottomColor;
        public Color IconColor;
        public Color IconPressedColor;
        public Color GaugeFillColor;
        public Color GaugeEmptyColor;
        public Vector3 NormalOffset;
        public Vector3 PressedOffset;
    }
}