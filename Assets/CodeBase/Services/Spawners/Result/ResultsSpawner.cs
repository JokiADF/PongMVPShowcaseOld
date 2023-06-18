using CodeBase.Presenters;
using CodeBase.Services.AssetManagement;
using UnityEngine;

namespace CodeBase.Services.Spawners.Result
{
    public class ResultsSpawner : IResultsSpawner
    {
        private readonly ResultsPresenter.Factory _factory;
        private readonly IAssetService _assetService;

        private ResultsPresenter _results;

        public ResultsSpawner(ResultsPresenter.Factory factory, IAssetService assetService)
        {
            _factory = factory;
            _assetService = assetService;
        }

        public void Spawn()
        {
            var prefab = _assetService.Get<GameObject>(AssetName.UI.Results);

            if (prefab == null)
                return;

            _results = _factory.Create(prefab);
        }

        public void Despawn()
        {
            if (_results == null)
                return;

            Object.Destroy(_results.gameObject);
            _results = null;
        }
    }
}