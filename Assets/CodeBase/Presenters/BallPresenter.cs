using CodeBase.Model;
using CodeBase.Services.AssetManagement;
using SpaceInvaders.Services;
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
        
        [Inject]
        private void Construct(BallModel ball, IAudioService audioService)
        {
            _ball = ball;
            _audioService = audioService;
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
                .Subscribe(collision =>
                {
                    _ball.Clash(collision);
                    
                    _audioService.PlaySfx(AssetName.Audio.Clash, 0.1f);
                })
                .AddTo(this);
        }

        private bool Move() =>
            _ball.Move(Time.fixedDeltaTime);

        public class Factory : PlaceholderFactory<Object, BallPresenter>
        {
        }
    }
}