using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCity.Source.Services
{
    public interface IAssetProvider : IService
    {
        Task<T> LoadAssetAsync<T>(AssetReference reference) where T : class;
        T LoadAsset<T>(string path) where T : Object;
        Task<T> LoadAssetAsync<T>(string address) where T : class;
        void CleanUp();
    }

    public class AssetProvider : IAssetProvider, IInitializableService
    {
        private readonly Dictionary<string, AsyncOperationHandle> _handlesCache = new();

        private readonly Dictionary<string, List<AsyncOperationHandle>> _tempHandlesCache
            = new();

        public void Initialize()
        {
            Addressables.InitializeAsync();
        }

        public async Task<T> LoadAssetAsync<T>(AssetReference reference) where T : class
        {
            if (_handlesCache.TryGetValue(reference.AssetGUID, out AsyncOperationHandle existingHandle))
                return existingHandle.Result as T;

            return await RunWithCacheOnComplete(Addressables.LoadAssetAsync<T>(reference), cacheKey: reference.AssetGUID);
        }

        public async Task<T> LoadAssetAsync<T>(string address) where T : class
        {
            if (_handlesCache.TryGetValue(address, out AsyncOperationHandle existingHandle))
                return existingHandle.Result as T;
           
            return await RunWithCacheOnComplete(Addressables.LoadAssetAsync<T>(address), cacheKey: address);
        }

        private async Task<T> RunWithCacheOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey) where T : class
        {
            handle.Completed += h => { _handlesCache[cacheKey] = h; };

            AddHandle(cacheKey, handle);
            return await handle.Task;
        }

        private void AddHandle<T>(string key, AsyncOperationHandle<T> handle) where T : class
        {
            if (!_tempHandlesCache.TryGetValue(key, out List<AsyncOperationHandle> handles))
            {
                handles = new();
                _tempHandlesCache[key] = handles;
            }

            handles.Add(handle);
        }

        public void CleanUp()
        {
            foreach (List<AsyncOperationHandle> handles in _tempHandlesCache.Values)
            {
                foreach (AsyncOperationHandle handle in handles)
                {
                    Addressables.Release(handle);
                }
            }
            
            _tempHandlesCache.Clear();
            _handlesCache.Clear();
        }

        public T LoadAsset<T>(string path) where T : Object
        {
            T asset = Resources.Load<T>(path);

            if (asset == null)
            {
                throw new FileLoadException($"The asset at path \"{path}\" doesn't exist or type mismatch");
            }

            return asset;
        }
    }
}