using CodeBase.Model;
using CodeBase.Services.Audio;
using CodeBase.Services.CameraShaker;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace CodeBase.Presenters
{
    public class PlayerPresenter : MonoBehaviour
    {
        [SerializeField] private ParticleSystem thrusterUp;
        [SerializeField] private ParticleSystem thrusterDown;
        
        private PlayerModel _player;
        private InputModel _input;
        private IAudioService _audioService;
        private AudioConfig _audioConfig;
        private ICameraShaker _cameraShaker;

        [Inject]
        private void Construct(PlayerModel player, InputModel input, IAudioService audioService, AudioConfig audioConfig,
            ICameraShaker cameraShaker)
        {
            _player = player;
            _input = input;
            
            _audioService = audioService;
            _audioConfig = audioConfig;

            _cameraShaker = cameraShaker;
        }
        
        private void Start()
        {
            _player.Reset();
            _player.Position
                .Subscribe(pos => transform.position = pos)
                .AddTo(this);

            Observable
                .EveryUpdate()
                .Subscribe(_ => Move())
                .AddTo(this);
            
            _player.Attempts
                .Pairwise()
                .Where(attempts => attempts.Current < attempts.Previous)
                .Subscribe(_ => OnHit())
                .AddTo(this);
        }

        private void OnHit()
        {
            if (!_player.IsInvulnerable.Value)
            {
                _cameraShaker.Shake(0.6f, 1.0f);
                _audioService.DuckMusic(0.025f, _audioConfig.musicVolume, 1.2f);
            }
            
            _player.DamageAsync().Forget();
        }

        private void Move()
        {
            _player.Move(_input.Vertical, Time.deltaTime);

            ManageThrusterParticles();
        }

        private void ManageThrusterParticles()
        {
            if (_input.Vertical > 0f)
            {
                StopThruster(thrusterUp);
                StartThruster(thrusterDown);
            }
            else if (_input.Vertical < 0f)
            {
                StopThruster(thrusterDown);
                StartThruster(thrusterUp);
            }
            else
            {
                StopThruster(thrusterUp);
                StopThruster(thrusterDown);
            }
        }

        private void StartThruster(ParticleSystem particle)
        {
            if (!particle.isPlaying) 
                particle.Play();
        }

        private void StopThruster(ParticleSystem particle)
        {
            if (particle.isPlaying) 
                particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        public class Factory : PlaceholderFactory<Object, PlayerPresenter>
        {
        }
    }
}
