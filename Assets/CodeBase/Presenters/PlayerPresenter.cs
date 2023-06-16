using CodeBase.Model;
using UniRx;
using UnityEngine;
using Zenject;

namespace CodeBase.Presenters
{
    public class PlayerPresenter : MonoBehaviour
    {
        private PlayerModel _player;
        private InputModel _input;

        [Inject]
        private void Construct(PlayerModel player, InputModel input)
        {
            _player = player;
            _input = input;
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
        }
        
        private void Move() => 
            _player.Move(_input.Vertical, Time.deltaTime);

        public class Factory : PlaceholderFactory<Object, PlayerPresenter>
        {
        }
    }
}
