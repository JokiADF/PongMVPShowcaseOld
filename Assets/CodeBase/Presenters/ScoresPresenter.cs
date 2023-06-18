using CodeBase.Helpers;
using CodeBase.Infrastructure.States;
using CodeBase.Model;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.Presenters
{
    public class ScoresPresenter : MonoBehaviour
    {
        [SerializeField] private Button buttonBack;
        [SerializeField] private TextMeshProUGUI itemPrefab;
        [SerializeField] private VerticalLayoutGroup itemHolder;

        private ScoresModel _scores;
        private GameStateMachine _gameState;

        private BoundItemsContainer<ScoreItem> _scoresContainer;

        [Inject]
        private void Construct(ScoresModel scores, GameStateMachine gameState)
        {
            _scores = scores;
            _gameState = gameState;
        }
        
        private void Start()
        {
            buttonBack.OnClickAsObservable()
                .Subscribe(_ => _gameState.Enter<LoadLobbyState>())
                .AddTo(this);

            _scoresContainer = new BoundItemsContainer<ScoreItem>(itemPrefab.gameObject, itemHolder.gameObject)
            {
                DestroyOnRemove = true
            };

            _scoresContainer.ObserveAdd().Subscribe(e =>
            {
                if (e.GameObject.TryGetComponent(out TextMeshProUGUI item))
                {
                    item.text = $"<color=#00ffff>{e.Model.score}</color> - {e.Model.date}";
                }
            }).AddTo(this);

            _scoresContainer.Initialize(_scores.Scoreboard);
            _scoresContainer.AddTo(this);
        }
    }
}
