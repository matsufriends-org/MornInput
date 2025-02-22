using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace MornInput
{
    [ExecuteAlways]
    public sealed class MornInputIconSetter : MonoBehaviour
    {
        [SerializeField] private MornInputIconSettings _settings;
        [SerializeField] private Image _image;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [Inject] private IMornInput _mornInput;

        private void OnEnable()
        {
            if (Application.isPlaying && _settings != null)
            {
                Adjust(_settings.DefaultSchemeKey);
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
            if (!Application.isPlaying && _settings != null)
            {
                Adjust(_settings.DefaultSchemeKey);
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
                MornInputUtil.Log("Image Changed");
                MornInputUtil.SetDirty(_image);
            }

            if (_spriteRenderer != null && _spriteRenderer.sprite != sprite)
            {
                _spriteRenderer.sprite = sprite;
                MornInputUtil.Log("SpriteRenderer Changed");
                MornInputUtil.SetDirty(_spriteRenderer);
            }
        }
    }
}