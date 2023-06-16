using System;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.Model
{
    [Serializable]
    public class BallConfig
    {
        [Tooltip("Speed in m/s.")]
        public float speed = 2.5f;
        [Tooltip("Position of the ball at start.")]
        public Vector3 spawnPosition = new Vector3(0f, 0f, -1f);
        [Tooltip("Rigidbody velocity")]
        public Vector3 velocity = Vector3.zero;
        [Tooltip("Rebound force")]
        public float bounceStrength = 5f;
    }

    public class BallModel
    {
        public Vector3ReactiveProperty Position { get; private set; }
        public Vector3ReactiveProperty Velocity { get; private set; }
    
        private readonly BallConfig _ballConfig;
        private readonly LevelConfig _levelConfig;
        private readonly GameplayModel _gameplay;

        public BallModel(BallConfig ballConfig, LevelConfig levelConfig, GameplayModel gameplay)
        {
            _ballConfig = ballConfig;
            _levelConfig = levelConfig;
            _gameplay = gameplay;

            Position = new Vector3ReactiveProperty(_ballConfig.spawnPosition);
            Velocity = new Vector3ReactiveProperty(_ballConfig.velocity);
        }

        public void Reset()
        {
            Position.Value = _ballConfig.spawnPosition;
            Velocity.Value = _ballConfig.velocity;
            
            AddStartingForce();
        }
    
        private void AddStartingForce()
        {
            var x = Random.value < 0.5f 
                ? -1f 
                : 1f;
            var y = Random.value < 0.5f 
                ? Random.Range(-1f, -0.5f)
                : Random.Range(0.5f, 1f);
            
            var direction = new Vector3(x, y);
            Velocity.Value += direction * _ballConfig.speed;
        }
        
        public void Move(float deltaTime)
        {
            var deltaPos = Velocity.Value * _ballConfig.speed * deltaTime;

            if (_levelConfig.IsPosOutOfHorizontalBounds(Position.Value + deltaPos))
            {
                if ((Position.Value + deltaPos).x > 0)
                    _gameplay.CurrentEnemyScore.Value++;
                else 
                    _gameplay.CurrentPlayerScore.Value++;
                
                Reset();
                
                return;
            }

            Position.Value += deltaPos;
        }

        public void Clash(Collision collision)
        {
            var normal = collision.GetContact(0).normal;
            Velocity.Value += normal.normalized * _ballConfig.bounceStrength; // * _ballConfig.impulseMagnitude
        }
    }
}