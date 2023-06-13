using CodeBase.Presenters;
using CodeBase.Services.AssetManagement;
using UnityEngine;

namespace CodeBase.Services.Spawners.Player
{
    public class PlayerSpawner : IPlayerSpawner
    {
        private readonly PlayerPresenter.Factory _factory;
        private readonly IAssetService _assetService;

        private PlayerPresenter _player;

        public PlayerSpawner(PlayerPresenter.Factory factory, IAssetService assetService)
        {
            _factory = factory;
            _assetService = assetService;
        }

        public void Spawn()
        {
            var prefab = _assetService.Get<GameObject>(AssetName.Objects.Player);

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
