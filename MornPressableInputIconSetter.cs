using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VContainer;
using System.Linq;

namespace MornInput
{
    [ExecuteAlways]
    internal sealed class MornPressableInputIconSetter : MonoBehaviour
    {
        public bool isPressed = false;

        public Color BackColor;
       
        [Header("normal")]
        [SerializeField] private MornInputIconSettings _normalSettings;
        [SerializeField] private MornInputIconSetter _normalBase;
        
        
        [Header("pressed")]
        [SerializeField] private MornInputIconSettings _pressedSettings;
        [SerializeField] private MornInputIconSetter _pressedBase;
        
        [Header("icon")]
        [SerializeField] private MornInputIconSettings _iconSettings;
        [SerializeField] private MornPressableInputIconSettings _iconOffsetSettings;
        [SerializeField] private MornInputIconSetter _icon;
        
        private bool _beforeIsPressed = false;
        
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
            _normalBase = GetComponentsInChildren<MornInputIconSetter>().FirstOrDefault(x => x.name == "Normal");
            _pressedBase = GetComponentsInChildren<MornInputIconSetter>().FirstOrDefault(x => x.name == "Pressed");
            _icon = GetComponentsInChildren<MornInputIconSetter>().FirstOrDefault(x => x.name == "Icon");
        }

        private void Update()
        {
            if (!Application.isPlaying)
            {
                Adjust(MornInputGlobal.I.DefaultSchemeKey);
            }
            
            if (_beforeIsPressed != isPressed)
            {
                SwitchPressed(isPressed);
                _beforeIsPressed = isPressed;
            }
            
            if(BackColor != _normalBase.IconColor)
            {
                _normalBase.IconColor = BackColor;
            }
            
            if(BackColor != _pressedBase.IconColor)
            {
                _pressedBase.IconColor = BackColor;
            }
        }
        
        public void SwitchPressed(bool isPressed)
        {
            this.isPressed = isPressed;
            
            if (_normalBase != null) _normalBase.gameObject.SetActive(!isPressed);
            if (_pressedBase != null) _pressedBase.gameObject.SetActive(isPressed);
            
            _icon.transform.localPosition = isPressed ? _iconOffsetSettings.PressedOffset : _iconOffsetSettings.NormalOffset;
            _icon.IconColor = isPressed ? _iconOffsetSettings.PressedColor : _iconOffsetSettings.NormalColor;
        }

        public void Adjust(string schemeKey)
        {
            
            if (_normalSettings != null && _normalBase.Settings != _normalSettings)
            {
                _normalBase.Settings = _normalSettings;
                MornInputGlobal.Log("Settings Changed");
                MornInputGlobal.SetDirty(_normalBase);
            }
            
            if (_pressedSettings != null && _pressedBase.Settings != _pressedSettings)
            {
                _pressedBase.Settings = _pressedSettings;
                MornInputGlobal.Log("Settings Changed");
                MornInputGlobal.SetDirty(_pressedBase);
            }
            
            if (_iconSettings != null && _icon.Settings != _iconSettings)
            {
                _icon.Settings = _iconSettings;
                MornInputGlobal.Log("Settings Changed");
                MornInputGlobal.SetDirty(_icon);
            }

            if (_normalBase.IconColor != BackColor)
            {
                _normalBase.IconColor = BackColor;
                MornInputGlobal.Log("IconColor Changed");
                MornInputGlobal.SetDirty(_normalBase);
            }
            
            if (_pressedBase.IconColor != BackColor)
            {
                _pressedBase.IconColor = BackColor;
                MornInputGlobal.Log("IconColor Changed");
                MornInputGlobal.SetDirty(_pressedBase);
            }

            var color = isPressed ? _iconOffsetSettings.PressedColor : _iconOffsetSettings.NormalColor;
            
            if (_icon.IconColor != color)
            {
                _icon.IconColor = color;
                MornInputGlobal.Log("IconColor Changed");
                MornInputGlobal.SetDirty(_icon);
            }
        }
    }
}