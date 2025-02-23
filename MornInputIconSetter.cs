using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace MornInput
{
    [ExecuteAlways]
    internal sealed class MornInputIconSetter : MonoBehaviour
    {
        [SerializeField] private MornInputIconSettings _settings;
        [SerializeField] private Image _image;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [Inject] private IMornInput _mornInput;

        private void OnEnable()
        {
            if (Application.isPlaying)
            {
                Adjust(MornInputGlobal.I.DefaultSchemeKey);
            }

            if (Application.isPlaying && _mornInput != null)
            {
                Adjust(_mornInput.CurrentScheme);
                _mornInput.OnSchemeChanged.Subscribe(x => Adjust(x.next)).AddTo(this);
            }
        }

        private void Reset()
        {
            _image = GetComponent<Image>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (!Application.isPlaying)
            {
                Adjust(MornInputGlobal.I.DefaultSchemeKey);
            }
        }

        public void Adjust(string schemeKey)
        {
            if (_settings == null)
            {
                return;
            }

            var sprite = _settings.GetSpriteSettings(schemeKey);
            if (sprite == null)
            {
                return;
            }
            
            if (_image != null && _image.sprite != sprite)
            {
                _image.sprite = sprite;
                MornInputGlobal.Log("Image Changed");
                MornInputGlobal.SetDirty(_image);
            }

            if (_spriteRenderer != null && _spriteRenderer.sprite != sprite)
            {
                _spriteRenderer.sprite = sprite;
                MornInputGlobal.Log("SpriteRenderer Changed");
                MornInputGlobal.SetDirty(_spriteRenderer);
            }
        }
    }
}