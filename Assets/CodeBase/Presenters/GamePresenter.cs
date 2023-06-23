using CodeBase.Model;
using CodeBase.Services.AssetManagement;
using CodeBase.Services.Audio;
using CodeBase.Services.Spawners.Ball;
using CodeBase.Services.Spawners.Enemy;
using CodeBase.Services.Spawners.Input;
using CodeBase.Services.Spawners.Player;
using UniRx;
using UnityEngine;
using Zenject;

namespace CodeBase.Presenters
{
    public class GamePresenter : MonoBehaviour
    {
        [SerializeField] private MeshRenderer background;

        private UGUIStateModel _stateMachine;
        private GameplayModel _gameplay;
        private IAssetService _assetService;
        
        private IPlayerSpawner _playerSpawner;
        private IEnemySpawner _enemySpawner;
        private IBallSpawner _ballSpawner;
        private IInputSpawner _inputSpawner;
        private IAudioService _audioService;

        private PlayerConfig _playerConfig;
        private AudioConfig _audioConfig;

        [Inject]
        private void Construct(UGUIStateModel stateMachine, GameplayModel gameplay, IAssetService assetService, IAudioService audioService,
            PlayerConfig playerConfig, AudioConfig audioConfig)
        {
            _stateMachine = stateMachine;
            _gameplay = gameplay;
            _assetService = assetService;
            _audioService = audioService;
            
            _playerConfig = playerConfig;
            _audioConfig = audioConfig;
        }

        [Inject]
        private void ConstructSpawners(IPlayerSpawner playerSpawner, IEnemySpawner enemySpawner,
            IBallSpawner ballSpawner, IInputSpawner inputSpawner)
        {
            _playerSpawner = playerSpawner;
            _enemySpawner = enemySpawner;
            _ballSpawner = ballSpawner;
            _inputSpawner = inputSpawner;
        }
        
        private void Start()
        {
            background.sharedMaterial = _assetService.Get<Material>(AssetName.Materials.Background);
            background.enabled = true;

            _stateMachine.State
                .Pairwise()
                .Subscribe(OnStateTransition)
                .AddTo(this);
            
            Observable.EveryUpdate()
                .Where(_ => _stateMachine.State.Value == UGUIState.Gameplay)
                .Subscribe(_ => GameplayLoop())
                .AddTo(this);
        }

        private void GameplayLoop()
        {
            DetectEndGame();
        }

        private void DetectEndGame()
        {
            if (_gameplay.CurrentEnemyScore.Value >= _playerConfig.attempts)
            {
                OnGameplayEnded();

                _stateMachine.State.Value = UGUIState.Results;
            }
        }

        private void OnStateTransition(Pair<UGUIState> transition)
        {
            if (transition.Current == UGUIState.Gameplay)
            {
                OnGameplayStarted();
            }
            else if (transition.Previous == UGUIState.Gameplay)
            {
                OnGameplayEnded();
            }
        }

        private void OnGameplayStarted()
        {
            _playerSpawner.Spawn();
            _enemySpawner.Spawn();
            _ballSpawner.Spawn();
            _inputSpawner.Spawn();
            
            _audioService.PlayMusic(AssetName.Audio.Music, _audioConfig.musicVolume);
        }

        private void OnGameplayEnded()
        {
            _playerSpawner.Despawn();
            _enemySpawner.Despawn();
            _ballSpawner.Despawn();
            _inputSpawner.Despawn();
            
            _audioService.StopMusic();
        }
    }
}
