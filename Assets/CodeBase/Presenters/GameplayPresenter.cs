using CodeBase.Helpers;
using CodeBase.Model;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace CodeBase.Presenters
{
    public class GameplayPresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI labelPlayerScore;
        [SerializeField] private TextMeshProUGUI labelEnemyScore;

        private GameplayModel _gameplay;

        [Inject]
        private void Construct(GameplayModel gameplay)
        {
            _gameplay = gameplay;
        }

        private void Start()
        {
            _gameplay.CurrentPlayerScore.SubscribeToText(labelPlayerScore).AddTo(this);
            _gameplay.CurrentEnemyScore.SubscribeToText(labelEnemyScore).AddTo(this);

            _gameplay.CurrentPlayerScore.Pairwise().Where(score => score.Current > score.Previous).Subscribe(_ =>
            {
                labelPlayerScore.rectTransform
                    .DOPunchScale(new Vector3(0.25f, 0.25f, 0f), 0.125f)
                    .SetEase(Ease.OutQuint)
                    .OnComplete(() => labelPlayerScore.rectTransform.localScale = Vector3.one);
            }).AddTo(this);
            _gameplay.CurrentEnemyScore.Pairwise().Where(score => score.Current > score.Previous).Subscribe(_ =>
            {
                labelEnemyScore.rectTransform
                    .DOPunchScale(new Vector3(0.25f, 0.25f, 0f), 0.125f)
                    .SetEase(Ease.OutQuint)
                    .OnComplete(() => labelEnemyScore.rectTransform.localScale = Vector3.one);
            }).AddTo(this);
        }
    }
}