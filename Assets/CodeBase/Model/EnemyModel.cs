using System;
using UniRx;
using UnityEngine;

namespace CodeBase.Model
{
    [Serializable]
    public class EnemyConfig
    {
        [Tooltip("Vertical speed in m/s.")]
        public float speed = 5.0f;
        [Tooltip("Position of the player at start.")]
        public Vector3 spawnPosition = new Vector3(-7.5f, 0f, -1f);
    }
    
    public class EnemyModel
    {
        public Vector3ReactiveProperty Position { get; private set; }
        
        private readonly EnemyConfig _enemyConfig;
        private readonly LevelConfig _levelConfig;

        public EnemyModel(EnemyConfig enemyConfig, LevelConfig levelConfig)
        {
            _enemyConfig = enemyConfig;
            _levelConfig = levelConfig;

            Position = new Vector3ReactiveProperty(_enemyConfig.spawnPosition);
        }

        public void Reset() => 
            Position.Value = _enemyConfig.spawnPosition;

        public void Move(Vector3 target, float deltaTime)
        {
            Vector3 pos = default;
            
            if (target.y > Position.Value.y)
                pos = Vector2.up;
            else if (target.y < Position.Value.y) 
                pos = Vector2.down;
            
            var deltaPos = pos * _enemyConfig.speed * deltaTime;

            if (_levelConfig.IsPosOutOfVerticalBounds(Position.Value + deltaPos))
                return;

            Position.Value += deltaPos;
        }
    }
}