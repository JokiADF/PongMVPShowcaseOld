using CodeBase.Presenters;
using CodeBase.Services.AssetManagement;
using UnityEngine;

namespace CodeBase.Services.Spawners.Ball
{
    public class BallSpawner : IBallSpawner
    {
        private readonly BallPresenter.Factory _factory;
        private readonly IAssetService _assetService;

        private BallPresenter _player;

        public BallSpawner(BallPresenter.Factory factory, IAssetService assetService)
        {
            _factory = factory;
            _assetService = assetService;
        }

        public void Spawn()
        {
            var prefab = _assetService.Get<GameObject>(AssetName.Objects.Ball);

            if (prefab == null)
                return;

            _player = _factory.Create(prefab);
        }

        public void Despawn()
        {
            if (_player == null)
                return;

            Object.Destroy(_player.gameObject);
            _player = null;
        }
    }
}