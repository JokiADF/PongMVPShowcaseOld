using CodeBase.Presenters;
using CodeBase.Services.AssetManagement;
using UnityEngine;

namespace CodeBase.Services.Spawners.Input
{
    public class InputSpawner : IInputSpawner
    {
        private readonly InputPresenter.Factory _factory;
        private readonly IAssetService _assetService;

        private InputPresenter _input;

        public InputSpawner(InputPresenter.Factory factory, IAssetService assetService)
        {
            _factory = factory;
            _assetService = assetService;
        }

        public void Spawn()
        {
            var prefab = _assetService.Get<GameObject>(AssetName.UI.Input);

            if (prefab == null)
                return;

            _input = _factory.Create(prefab);
        }

        public void Despawn()
        {
            if (_input == null)
                return;

            Object.Destroy(_input.gameObject);
            _input = null;
        }
    }
}