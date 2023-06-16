using CodeBase.Model;
using UniRx;
using UnityEngine;
using Zenject;

namespace CodeBase.Presenters
{
    public class BallPresenter : MonoBehaviour
    {
        private BallModel _ball;
        
        [Inject]
        private void Construct(BallModel ball)
        {
            _ball = ball;
        }
        
        private void Start()
        {
            _ball.Reset();
            _ball.Position
                .Subscribe(pos => transform.position = pos)
                .AddTo(this);
            
            Observable
                .EveryFixedUpdate()
                .Subscribe(_ => Move())
                .AddTo(this);
        }

        private void Move() =>
            _ball.Move(Time.fixedDeltaTime);

        private void OnCollisionEnter(Collision collision) => 
            _ball.Clash(collision);

        public class Factory : PlaceholderFactory<UnityEngine.Object, BallPresenter>
        {
        }
    }
}