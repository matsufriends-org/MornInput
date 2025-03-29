using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VContainer;

namespace MornInput
{
    [ExecuteAlways]
    internal sealed class MornInputIconSetter : MonoBehaviour
    {
        private static readonly int _fillAmount = Shader.PropertyToID("_FillAmount");
        [FormerlySerializedAs("_settings")] public MornInputIconSettings Settings;
        [SerializeField] private Image _image;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private SpriteMask _spriteMask;
        [Inject] private IMornInput _mornInput;
        public Color IconColor
        {
            get => _image != null ? _image.color : _spriteRenderer != null ?_spriteRenderer.color : Color.clear;
            set
            {
                if (_image != null)
                {
                    _image.color = value;
                }
                else if (_spriteRenderer != null)
                {
                    _spriteRenderer.color = value;
                }
            }
        }
        private MaterialPropertyBlock _propertyBlock;

        private void Awake()
        {
            if (_spriteRenderer != null)
            {
                _propertyBlock = new MaterialPropertyBlock(); 
                _spriteRenderer.SetPropertyBlock(_propertyBlock);
            }
        }

        public float FillAmount
        {
            set
            {
                if (_image != null)
                {
                    _image.fillAmount = value;
                }
                else if (_spriteRenderer != null)
                {
                    _propertyBlock.SetFloat(_fillAmount, value);
                    _spriteRenderer.SetPropertyBlock(_propertyBlock);
                }
            }
        }

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
            _spriteMask = GetComponent<SpriteMask>();
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
            if (Settings == null)
            {
                return;
            }

            var sprite = Settings.GetSpriteSettings(schemeKey);
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
            
            if (_spriteMask != null && _spriteMask.sprite != sprite)
            {
                _spriteMask.sprite = sprite;
                MornInputGlobal.Log("SpriteMask Changed");
                MornInputGlobal.SetDirty(_spriteMask);
            }
        }
    }
}