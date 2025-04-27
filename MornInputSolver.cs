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
        string IMornInput.CurrentScheme => _playerInput.currentControlScheme;
        IObservable<(string prev, string next)> IMornInput.OnSchemeChanged => _schemeSubject;

        private void Reset()
        {
            _playerInput = GetComponent<PlayerInput>();
        }

        private void Update()
        {
            var currentControlScheme = _playerInput.currentControlScheme;
            if (_cachedControlScheme == currentControlScheme)
            {
                return;
            }

            _schemeSubject.OnNext((_cachedControlScheme, currentControlScheme));
            MornInputGlobal.Log($"ControlScheme changed: {_cachedControlScheme ?? "None"} -> {currentControlScheme}");
            _cachedControlScheme = currentControlScheme;
        }

        bool IMornInput.IsPressedAny(string actionName)
        {
            return GetAction(actionName).AnyPressed();
        }

        bool IMornInput.IsPressedAll(string actionName)
        {
            return GetAction(actionName).AllPressed();
        }

        bool IMornInput.IsPerformed(string actionName)
        {
            return GetAction(actionName).WasPerformedThisFrame();
        }

        bool IMornInput.IsPressingAny(string actionName)
        {
            return GetAction(actionName).AnyPressing();
        }

        bool IMornInput.IsPressingAll(string actionName)
        {
            return GetAction(actionName).AllPressing();
        }

        bool IMornInput.IsReleaseAny(string actionName)
        {
            return GetAction(actionName).AnyReleased();
        }

        bool IMornInput.IsReleaseAll(string actionName)
        {
            return GetAction(actionName).AllReleased();
        }

        T IMornInput.ReadValue<T>(string actionName)
        {
            return GetAction(actionName).ReadValue<T>();
        }

        private InputAction GetAction(string actionName)
        {
            if (_cachedActionDictionary.TryGetValue(actionName, out var action))
            {
                return action;
            }

            action = _playerInput.actions[actionName];
            _cachedActionDictionary[actionName] = action;
            return action;
        }
    }
}