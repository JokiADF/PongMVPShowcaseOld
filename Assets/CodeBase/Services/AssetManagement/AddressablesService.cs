using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CodeBase.Services.AssetManagement
{
    public class AddressablesService : IAssetService, IDisposable
    {
        private readonly Dictionary<string, object> _assets = new();
        private readonly List<AsyncOperationHandle> _handles = new();

        public async UniTask Load<T>(string key)
        {
            if (string.IsNullOrEmpty(key) || _assets.ContainsKey(key))
            {
                return;
            }

            try
            {
                var handle = Addressables.LoadAssetAsync<T>(key);
                _handles.Add(handle);

                var obj = await handle.ToUniTask();
                _assets.Add(key, obj);
            }
            catch (InvalidKeyException exception)
            {
                Debug.LogException(exception);
            }
        }

        public T Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key) || !_assets.TryGetValue(key, out var obj))
                return default;

            return obj is T t ? t : default;
        }

        public void Dispose()
        {
            foreach (var handle in _handles) Addressables.Release(handle);
        }
    }
}
