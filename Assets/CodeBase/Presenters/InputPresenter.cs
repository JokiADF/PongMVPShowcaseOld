using CodeBase.Helpers;
using CodeBase.Infrastructure.States;
using CodeBase.Model;
using UniRx;
using UnityEngine;
using Zenject;

namespace CodeBase.Presenters
{
    public class InputPresenter : MonoBehaviour
    {
        [SerializeField] private ButtonAxis buttonVertical;

        private GameStateMachine _stateMachine;
        private InputModel _input;

        private const string AxisVertical = "Vertical";
        
        [Inject]
        private void Construct(GameStateMachine stateMachine, InputModel input)
        {
            _stateMachine = stateMachine;
            _input = input;
        }
        
        private void Start()
        {
#if UNITY_EDITOR
            buttonVertical.gameObject.SetActive(false);
#endif

            Observable.EveryUpdate().Where(_ => _stateMachine.ActiveStateType == typeof(GameLoopState)).Subscribe(_ =>
            {
#if UNITY_EDITOR
                _input.Vertical = Input.GetAxisRaw(AxisVertical);
#else
                _input.Vertical = buttonVertical.Axis;
#endif
            }).AddTo(this);
        }
    }
}