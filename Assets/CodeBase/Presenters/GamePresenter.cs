using CodeBase.Infrastructure.States;
using CodeBase.Model;
using CodeBase.Services.AssetManagement;
using CodeBase.Services.Spawners.Ball;
using CodeBase.Services.Spawners.Enemy;
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
        private GameplayModel _gameplay;
        
        private IPlayerSpawner _playerSpawner;
        private IEnemySpawner _enemySpawner;
        private IBallSpawner _ballSpawner;

        private PlayerConfig _playerConfig;

        [Inject]
        private void Construct(GameStateMachine stateMachine, IAssetService assetService, GameplayModel gameplay, 
            IPlayerSpawner playerSpawner, IEnemySpawner enemySpawner, IBallSpawner ballSpawner,
            PlayerConfig playerConfig)
        {
            _stateMachine = stateMachine;
            _assetService = assetService;
            _gameplay = gameplay;
            
            _playerSpawner = playerSpawner;
            _enemySpawner = enemySpawner;
            _ballSpawner = ballSpawner;

            _playerConfig = playerConfig;
        }
        
        private void Start()
        {
            background.sharedMaterial = _assetService.Get<Material>(AssetName.Materials.Background);
            background.enabled = true;

            OnGameplayStarted();
            
            Observable.EveryUpdate()
                .Where(_ => _stateMachine.ActiveStateType == typeof(GameLoopState))
                .Subscribe(_ => GameplayLoop())
                .AddTo(this);
        }

        private void GameplayLoop()
        {
            DetectEndGame();
        }

        private void OnGameplayStarted()
        {
            _playerSpawner.Spawn();
            _enemySpawner.Spawn();
            _ballSpawner.Spawn();
        }

        private void OnGameplayEnded()
        {
            _playerSpawner.Despawn();
            _enemySpawner.Despawn();
            _ballSpawner.Despawn();
        }
        
        private void DetectEndGame()
        {
            if (_gameplay.CurrentEnemyScore.Value >= _playerConfig.attempts)
            {
                OnGameplayEnded();
                
                _stateMachine.Enter<BootstrapState>();
            }
        }
    }
}
