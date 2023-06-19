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
        private UGUIStateModel _stateMachine;

        private BoundItemsContainer<ScoreItem> _scoresContainer;

        [Inject]
        private void Construct(ScoresModel scores, UGUIStateModel stateMachine)
        {
            _scores = scores;
            _stateMachine = stateMachine;
        }
        
        private void Start()
        {
            buttonBack.OnClickAsObservable()
                .Subscribe(_ => _stateMachine.State.Value = UGUIState.Menu)
                .AddTo(this);

            _scoresContainer = new BoundItemsContainer<ScoreItem>(itemPrefab.gameObject, itemHolder.gameObject)
            {
                DestroyOnRemove = true
            };

            _scoresContainer
                .ObserveAdd()
                .Subscribe(text =>
                {
                    if (text.GameObject.TryGetComponent(out TextMeshProUGUI item))
                    {
                        item.text = $"<color=#00ffff>{text.Model.score}</color> - {text.Model.date}";
                    }
                })
                .AddTo(this);

            _scoresContainer.Initialize(_scores.Scoreboard);
            _scoresContainer.AddTo(this);
        }
    }
}
