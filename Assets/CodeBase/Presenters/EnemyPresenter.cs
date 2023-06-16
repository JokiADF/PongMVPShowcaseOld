using CodeBase.Model;
using UniRx;
using UnityEngine;
using Zenject;

namespace CodeBase.Presenters
{
    public class EnemyPresenter : MonoBehaviour
    {
        private EnemyModel _enemy;
        private BallModel _ball;

        [Inject]
        private void Construct(EnemyModel enemy, BallModel ball)
        {
            _enemy = enemy;
            _ball = ball;
        }
        
        private void Start()
        {
            _enemy.Reset();
            _enemy.Position
                .Subscribe(pos => transform.position = pos)
                .AddTo(this);

            Observable
                .EveryUpdate()
                .Subscribe(_ => Move())
                .AddTo(this);
        }
        
        private void Move() => 
            _enemy.Move(_ball.Position.Value, Time.deltaTime);

        public class Factory : PlaceholderFactory<Object, EnemyPresenter>
        {
        }
    }
}