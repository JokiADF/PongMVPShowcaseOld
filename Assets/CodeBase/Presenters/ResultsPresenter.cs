using System;
using CodeBase.Helpers;
using CodeBase.Model;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.Presenters
{
    public class ResultsPresenter : MonoBehaviour
    {
        [SerializeField] private Button buttonBack;
        [SerializeField] private TextMeshProUGUI labelScore;

        private UGUIStateModel _stateMachine;
        private GameplayModel _gameplay;
        private ScoresModel _scores;
        
        [Inject]
        private void Construct(UGUIStateModel stateMachine, GameplayModel gameplay, ScoresModel scores)
        {
            _stateMachine = stateMachine;
            _gameplay = gameplay;
            _scores = scores;
        }
        
        private void Start()
        {
            buttonBack.OnClickAsObservable()
                .Subscribe(_ => _stateMachine.State.Value = UGUIState.Menu)
                .AddTo(this);

            _gameplay.CurrentPlayerScore.SubscribeToText(labelScore).AddTo(this);

            _stateMachine.State
                .Where(state => state == UGUIState.Results && _gameplay.CurrentPlayerScore.Value > 0)
                .Subscribe(_ => AddAndSaveScore())
                .AddTo(this);
        }

        private void AddAndSaveScore()
        {
            _scores.Add(new ScoreItem()
            {
                score = _gameplay.CurrentPlayerScore.Value,
                date = DateTime.Now.ToString("MM/dd/yyyy HH:mm"),
            });

            _scores.Save();
        }
    }
}
