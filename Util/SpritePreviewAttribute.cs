using UnityEngine;

namespace MornInput
{
    public sealed class SpritePreviewAttribute : PropertyAttribute
    {
        public readonly float Size;

        public SpritePreviewAttribute(float size = 30)
        {
            Size = size;
        }
    }
}