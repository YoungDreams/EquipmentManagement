using System;

namespace Foundation.Core
{
    public interface ICache
    {
        TValue GetOrAdd<TValue>(string key, Func<TValue> valueCreator);
        TValue GetOrAdd<TValue>(string key, Func<TValue> valueCreator, TimeSpan timeout);
        void Add<TValue>(string key, TValue value);
        void Add<TValue>(string key, TValue value, TimeSpan timeout);
        TValue Get<TValue>(string key);
        void Remove(string key);
    }
}
