using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MornInput
{
    [RequireComponent(typeof(PlayerInput))]
    public sealed class MornInputSolver : MonoBehaviour, IMornInput
    {
        [SerializeField] private PlayerInput _playerInput;
        private readonly Dictionary<string, InputAction> _cachedActionDictionary = new();
        private readonly Subject<(string prev, string next)> _schemeSubject = new();
        private string _cachedControlScheme;

        private void Reset()
        {
            _playerInput = GetComponent<PlayerInput>();
        }

        private void Update()
        {
            var currentControlScheme = _playerInput.currentControlScheme;
            if (_cachedControlScheme == currentControlScheme) return;
            _schemeSubject.OnNext((_cachedControlScheme, currentControlScheme));
            MornInputGlobal.Log($"ControlScheme changed: {_cachedControlScheme ?? "None"} -> {currentControlScheme}");
            _cachedControlScheme = currentControlScheme;
        }

        string IMornInput.CurrentScheme => _playerInput.currentControlScheme;
        IObservable<(string prev, string next)> IMornInput.OnSchemeChanged => _schemeSubject;

        bool IMornInput.IsPressStart(string actionName)
        {
            return GetAction(actionName).WasPressedThisFrame();
        }

        bool IMornInput.IsPressing(string actionName)
        {
            return GetAction(actionName).IsPressed();
        }
        
        bool IMornInput.IsPerformed(string actionName)
        {
            return GetAction(actionName).WasPerformedThisFrame();
        }

        bool IMornInput.IsPressEnd(string actionName)
        {
            return GetAction(actionName).WasReleasedThisFrame();
        }

        T IMornInput.ReadValue<T>(string actionName)
        {
            return GetAction(actionName).ReadValue<T>();
        }

        private InputAction GetAction(string actionName)
        {
            if (_cachedActionDictionary.TryGetValue(actionName, out var action)) return action;
            action = _playerInput.actions[actionName];
            _cachedActionDictionary[actionName] = action;
            return action;
        }
    }
}