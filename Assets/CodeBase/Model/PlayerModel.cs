using System;
using UniRx;
using UnityEngine;

namespace CodeBase.Model
{
    [Serializable]
    public class PlayerConfig
    {
        [Tooltip("Number of attempts.")]
        public int attempts = 10;
        [Tooltip("Vertical speed in m/s.")]
        public float speed = 5.0f;
        [Tooltip("Position of the player at start.")]
        public Vector3 spawnPosition = new Vector3(7.5f, 0f, -1f);
    }

    [Serializable]
    public class LevelConfig
    {
        public Vector3 bounds = new Vector3(7.5f, 3.5f, 11f);
        
        private const float RefAspectRatio = 16f / 9;
        private readonly float _currentAspectRatio = Screen.width / (float)Screen.height;

        public bool IsPosOutOfHorizontalBounds(Vector3 pos)
        {
            var boundX = bounds.x * (_currentAspectRatio / RefAspectRatio);

            return pos.x > boundX || pos.x < -boundX;
        }

        public bool IsPosOutOfVerticalBounds(Vector3 pos)
        {
            var boundY = bounds.y * (_currentAspectRatio / RefAspectRatio);
            
            return pos.y > boundY || pos.y < -boundY;
        }
    }
    
    public class PlayerModel
    {
        public Vector3ReactiveProperty Position { get; private set; }
        public ReactiveProperty<int> Attempts { get; private set; }
        public ReadOnlyReactiveProperty<bool> IsDead { get; private set; }
        
        private readonly PlayerConfig _playerConfig;
        private readonly LevelConfig _levelConfig;

        public PlayerModel(PlayerConfig playerConfig, LevelConfig levelConfig)
        {
            _playerConfig = playerConfig;
            _levelConfig = levelConfig;

            Position = new Vector3ReactiveProperty(_playerConfig.spawnPosition);
            Attempts = new ReactiveProperty<int>(_playerConfig.attempts);

            IsDead = Attempts
                .Select(attempts => attempts <= 0)
                .ToReadOnlyReactiveProperty();
        }

        public void Reset()
        {
            Position.Value = _playerConfig.spawnPosition;
            Attempts.Value = _playerConfig.attempts;
        }

        public void Move(float vertical, float deltaTime)
        {
            var deltaPos = vertical * _playerConfig.speed * Vector3.up * deltaTime;

            if (_levelConfig.IsPosOutOfVerticalBounds(Position.Value + deltaPos))
                return;

            Position.Value += deltaPos;
        }
    }
}
