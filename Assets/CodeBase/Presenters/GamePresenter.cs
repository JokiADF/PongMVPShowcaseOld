using CodeBase.Infrastructure.States;
using CodeBase.Services.AssetManagement;
using CodeBase.Services.Spawners.Ball;
using CodeBase.Services.Spawners.Player;
using UniRx;
using UnityEngine;
using Zenject;

namespace CodeBase.Presenters
{
    public class GamePresenter : MonoBehaviour
    {
        [SerializeField] private MeshRenderer background;

        private GameStateMachine _stateMachine;
        private IAssetService _assetService;
        private IPlayerSpawner _playerSpawner;
        private IBallSpawner _ballSpawner;

        [Inject]
        private void Construct(GameStateMachine stateMachine, IAssetService assetService, IPlayerSpawner playerSpawner, IBallSpawner ballSpawner)
        {
            _stateMachine = stateMachine;
            _assetService = assetService;
            _playerSpawner = playerSpawner;
            _ballSpawner = ballSpawner;
        }
        
        private void Start()
        {
            background.sharedMaterial = _assetService.Get<Material>(AssetName.Materials.Background);
            background.enabled = true;

            OnGameplayStarted();
            
            // Control enemies during gameplay
            Observable.EveryUpdate()
                .Where(_ => _stateMachine.ActiveStateType == typeof(GameLoopState))
                .Subscribe(_ => GameplayLoop())
                .AddTo(this);
        }

        private void GameplayLoop()
        {}

        private void OnGameplayStarted()
        {
            _playerSpawner.Spawn();
            _ballSpawner.Spawn();
        }

        private void OnGameplayEnded()
        {
            _playerSpawner.Despawn();
            _ballSpawner.Despawn();
        }
    }
}
