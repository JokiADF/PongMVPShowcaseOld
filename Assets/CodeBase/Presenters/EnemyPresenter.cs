using CodeBase.Model;
using UniRx;
using UnityEngine;
using Zenject;

namespace CodeBase.Presenters
{
    public class EnemyPresenter : MonoBehaviour
    {
        [SerializeField] private ParticleSystem thrusterUp;
        [SerializeField] private ParticleSystem thrusterDown;
        
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
        
        private void Move()
        {
            _enemy.Move(_ball.Position.Value, Time.deltaTime);
            
            ManageThrusterParticles(_ball.Position.Value);
        }

        private void ManageThrusterParticles(Vector3 movePos)
        {
            if (movePos.y > transform.position.y)
            {
                StopThruster(thrusterUp);
                StartThruster(thrusterDown);
            }
            else if (movePos.y < transform.position.y)
            {
                StartThruster(thrusterUp);
                StopThruster(thrusterDown);
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

        public class Factory : PlaceholderFactory<Object, EnemyPresenter>
        {
        }
    }
}