using CodeBase.Model;
using CodeBase.Services.AssetManagement;
using CodeBase.Services.Audio;
using CodeBase.Services.Spawners.Explosion;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace CodeBase.Presenters
{
    public class BallPresenter : MonoBehaviour
    {
        private BallModel _ball;
        private IAudioService _audioService;
        private IExplosionSpawner _explosionSpawner;

        [Inject]
        private void Construct(BallModel ball, IAudioService audioService, IExplosionSpawner explosionSpawner)
        {
            _ball = ball;
            _audioService = audioService;
            _explosionSpawner = explosionSpawner;
        }
        
        private void Start()
        {
            _ball.Reset();
            _ball.Position
                .Subscribe(pos => transform.position = pos)
                .AddTo(this);
            
            Observable
                .EveryFixedUpdate()
                .Subscribe(_ =>
                {
                    if(Move())
                        _audioService.PlaySfx(AssetName.Audio.Explosion, 0.25f);
                })
                .AddTo(this);

            this
                .OnCollisionEnterAsObservable()
                .Subscribe(Clash)
                .AddTo(this);
        }

        private void Clash(Collision collision)
        {
            _ball.Clash(collision);

            _audioService.PlaySfx(AssetName.Audio.Clash, 0.1f);
            _explosionSpawner.Spawn(transform.position);
        }

        private bool Move() =>
            _ball.Move(Time.fixedDeltaTime);

        public class Factory : PlaceholderFactory<Object, BallPresenter>
        {
        }
    }
}