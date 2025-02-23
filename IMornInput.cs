using System;

namespace MornInput
{
    public interface IMornInput
    {
        string CurrentScheme { get; }
        IObservable<(string prev, string next)> OnSchemeChanged { get; }
        bool IsPressStart(string actionName);
        bool IsPressing(string actionName);
        bool IsPerformed(string actionName);
        bool IsPressEnd(string actionName);
        T ReadValue<T>(string actionName) where T : struct;
    }
}