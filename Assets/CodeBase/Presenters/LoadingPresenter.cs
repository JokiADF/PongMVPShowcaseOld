using System;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;

namespace CodeBase.Presenters
{
    public class LoadingPresenter : MonoBehaviour
    {
        [SerializeField] private CanvasGroup group;
        [SerializeField] private TextMeshProUGUI text;

        private readonly string[] _loadingTexts = new string[4]
        {
            "Loading", "Loading.", "Loading..", "Loading..."
        };
        
        private void Awake() => 
            DontDestroyOnLoad(this);

        public void Show()
        {
            gameObject.SetActive(true);
            group.alpha = 1;
            
            UpdateLoadingText();
        }

        public void Hide() =>
            group.DOFade(0, 0.5f)
                .SetEase(Ease.Linear)
                .OnComplete(() => gameObject.SetActive(false));

        private void UpdateLoadingText() =>
            Observable.Timer(TimeSpan.FromSeconds(0.25f), TimeSpan.FromSeconds(0.25))
                .Where(_ => gameObject.activeSelf)
                .Subscribe(tick => text.text = _loadingTexts[tick % _loadingTexts.Length])
                .AddTo(this);
    }
}
