using System;
using UnityEngine;

namespace MornInput
{
    [CreateAssetMenu(fileName = nameof(MornInputIconSettings), menuName = "Morn/" + nameof(MornInputIconSettings))]
    public sealed class MornInputIconSettings : ScriptableObject
    {
        [Serializable]
        private class SpriteSet
        {
            public string SchemeKey;
            [SpritePreview] public Sprite Sprite;
        }

        [SerializeField] private SpriteSet[] _spriteSets;
        public string DefaultSchemeKey;

        public Sprite GetSpriteSettings(string schemeKey)
        {
            foreach (var spriteSet in _spriteSets)
            {
                if (spriteSet.SchemeKey == schemeKey)
                {
                    return spriteSet.Sprite;
                }
            }
            return null;
        }
    }
}