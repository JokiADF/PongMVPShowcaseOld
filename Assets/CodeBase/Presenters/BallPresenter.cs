using CodeBase.Model;
using UniRx;
using UnityEngine;

namespace CodeBase.Presenters
{
    public class BallPresenter : MonoBehaviour
    {
        private BallModel _ball;
        private BallConfig _ballConfig;
        //
        // [Inject]
        // private void Construct(BallModel ball, BallConfig ballConfig)
        // {
        //     _ball = ball;
        //     _ballConfig = ballConfig;
        // }
        
        private void Start()
        {
            _ballConfig = new BallConfig();
            _ball = new BallModel(_ballConfig, new LevelConfig());
            
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
    }
}