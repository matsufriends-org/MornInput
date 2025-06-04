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
            try
            {
                if (_playerInput == null)
                {
                    MornInputGlobal.LogError("PlayerInput is null in MornInputSolver.Update");
                    return;
                }

                var currentControlScheme = _playerInput.currentControlScheme;
                if (_cachedControlScheme == currentControlScheme)
                {
                    return;
                }

                _schemeSubject.OnNext((_cachedControlScheme, currentControlScheme));
                MornInputGlobal.Log($"ControlScheme changed: {_cachedControlScheme ?? "None"} -> {currentControlScheme}");
                _cachedControlScheme = currentControlScheme;
            }
            catch (Exception ex)
            {
                MornInputGlobal.LogError($"Error in MornInputSolver.Update: {ex.Message}");
            }
        }

        bool IMornInput.IsPressedAny(string actionName)
        {
            var action = GetAction(actionName);
            return action?.AnyPressed() ?? false;
        }

        bool IMornInput.IsPressedAll(string actionName)
        {
            var action = GetAction(actionName);
            return action?.AllPressed() ?? false;
        }

        bool IMornInput.IsPerformed(string actionName)
        {
            var action = GetAction(actionName);
            return action?.WasPerformedThisFrame() ?? false;
        }

        bool IMornInput.IsPressingAny(string actionName)
        {
            var action = GetAction(actionName);
            return action?.AnyPressing() ?? false;
        }

        bool IMornInput.IsPressingAll(string actionName)
        {
            var action = GetAction(actionName);
            return action?.AllPressing() ?? false;
        }

        bool IMornInput.IsReleaseAny(string actionName)
        {
            var action = GetAction(actionName);
            return action?.AnyReleased() ?? false;
        }

        bool IMornInput.IsReleaseAll(string actionName)
        {
            var action = GetAction(actionName);
            return action?.AllReleased() ?? false;
        }

        T IMornInput.ReadValue<T>(string actionName)
        {
            var action = GetAction(actionName);
            return action != null ? action.ReadValue<T>() : default(T);
        }

        private InputAction GetAction(string actionName)
        {
            try
            {
                if (string.IsNullOrEmpty(actionName))
                {
                    MornInputGlobal.LogError("Action name is null or empty");
                    return null;
                }

                if (_cachedActionDictionary.TryGetValue(actionName, out var action))
                {
                    return action;
                }

                if (_playerInput?.actions == null)
                {
                    MornInputGlobal.LogError("PlayerInput or actions is null");
                    return null;
                }

                action = _playerInput.actions[actionName];
                if (action == null)
                {
                    MornInputGlobal.LogWarning($"Action '{actionName}' not found in PlayerInput");
                    return null;
                }

                _cachedActionDictionary[actionName] = action;
                return action;
            }
            catch (Exception ex)
            {
                MornInputGlobal.LogError($"Error getting action '{actionName}': {ex.Message}");
                return null;
            }
        }
    }
}