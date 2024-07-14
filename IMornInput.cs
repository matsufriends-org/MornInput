using System;

namespace MornInput
{
    public interface IMornInput
    {
        IObservable<(string prev, string next)> OnSchemeChanged { get; }
    }
}