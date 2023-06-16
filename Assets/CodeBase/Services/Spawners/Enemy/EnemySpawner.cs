using CodeBase.Presenters;
using CodeBase.Services.AssetManagement;
using UnityEngine;

namespace CodeBase.Services.Spawners.Enemy
{
    public class EnemySpawner : IEnemySpawner
    {
        private readonly EnemyPresenter.Factory _factory;
        private readonly IAssetService _assetService;

        private EnemyPresenter _enemy;

        public EnemySpawner(EnemyPresenter.Factory factory, IAssetService assetService)
        {
            _factory = factory;
            _assetService = assetService;
        }

        public void Spawn()
        {
            var prefab = _assetService.Get<GameObject>(AssetName.Objects.Enemy);

            if (prefab == null)
                return;

            _enemy = _factory.Create(prefab);
        }

        public void Despawn()
        {
            if (_enemy == null)
                return;

            Object.Destroy(_enemy.gameObject);
            _enemy = null;
        }
    }
}