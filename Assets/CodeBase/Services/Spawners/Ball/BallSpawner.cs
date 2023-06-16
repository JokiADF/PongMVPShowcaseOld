using CodeBase.Presenters;
using CodeBase.Services.AssetManagement;
using UnityEngine;

namespace CodeBase.Services.Spawners.Ball
{
    public class BallSpawner : IBallSpawner
    {
        private readonly BallPresenter.Factory _factory;
        private readonly IAssetService _assetService;

        private BallPresenter _ball;

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

            _ball = _factory.Create(prefab);
        }

        public void Despawn()
        {
            if (_ball == null)
                return;

            Object.Destroy(_ball.gameObject);
            _ball = null;
        }
    }
}