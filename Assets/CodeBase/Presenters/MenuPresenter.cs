using CodeBase.Model;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.Presenters
{
    public class MenuPresenter : MonoBehaviour
    {
        [SerializeField] private Button buttonPlay;
        [SerializeField] private Button buttonScores;
        
        private UGUIStateModel _stateMachine;

        [Inject]
        private void Construct(UGUIStateModel stateMachine)
        {
            _stateMachine = stateMachine;
        }

        private void Start()
        {
            buttonPlay.OnClickAsObservable()
                .Subscribe(_ => _stateMachine.State.Value = UGUIState.Gameplay)
                .AddTo(this);
            
            buttonScores.OnClickAsObservable()
                .Subscribe(_ => _stateMachine.State.Value = UGUIState.Scores)
                .AddTo(this);
        }
    }
}