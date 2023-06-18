using CodeBase.Presenters;
using CodeBase.Services.AssetManagement;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.Spawners.Gameplay
{
    public class GameplaySpawner : IGameplaySpawner
    {
        private readonly GameplayPresenter.Factory _factory;
        private readonly IAssetService _assetService;

        private GameplayPresenter _gameplay;

        public GameplaySpawner(GameplayPresenter.Factory factory, IAssetService assetService)
        {
            _factory = factory;
            _assetService = assetService;
        }

        public void Spawn()
        {
            var prefab = _assetService.Get<GameObject>(AssetName.UI.Gameplay);

            if (prefab == null)
                return;

            _gameplay = _factory.Create(prefab);
        }

        public void Despawn()
        {
            if (_gameplay == null)
                return;

            Object.Destroy(_gameplay.gameObject);
            _gameplay = null;
        }
    }
}