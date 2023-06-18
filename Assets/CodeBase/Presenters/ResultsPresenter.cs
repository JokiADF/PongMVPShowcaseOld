using System;
using CodeBase.Helpers;
using CodeBase.Infrastructure.States;
using CodeBase.Model;
using CodeBase.Services.Spawners.Result;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Object = UnityEngine.Object;

namespace CodeBase.Presenters
{
    public class ResultsPresenter : MonoBehaviour
    {
        [SerializeField] private Button buttonBack;
        [SerializeField] private TextMeshProUGUI labelScore;

        private GameStateMachine _gameState;
        private GameplayModel _gameplay;
        private ScoresModel _scores;
        private IResultsSpawner _resultsSpawner;
        
        [Inject]
        private void Construct(GameStateMachine gameState, GameplayModel gameplay, ScoresModel scores, IResultsSpawner resultsSpawner)
        {
            _gameState = gameState;
            _gameplay = gameplay;
            _scores = scores;
            _resultsSpawner = resultsSpawner;
        }
        
        private void Start()
        {
            buttonBack.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _gameState.Enter<BootstrapState>();
                    
                    _resultsSpawner.Despawn();
                })
                .AddTo(this);

            _gameplay.CurrentPlayerScore.SubscribeToText(labelScore).AddTo(this);

            _gameState.ActiveStateType
                .Where(state => state == typeof(ResultLoopState) && _gameplay.CurrentPlayerScore.Value > 0)
                .Subscribe(_ => AddAndSaveScore())
                .AddTo(this);
        }

        private void AddAndSaveScore()
        {
            var scoreItem = new ScoreItem()
            {
                score = _gameplay.CurrentPlayerScore.Value,
                date = DateTime.Now.ToString("MM/dd/yyyy HH:mm"),
            };

            _scores.Save();
        }

        public class Factory : PlaceholderFactory<Object, ResultsPresenter>
        {
        }
    }
}
