using CodeBase.Helpers;
using CodeBase.Model;
using CodeBase.Services.Audio;
using DG.Tweening;
using SpaceInvaders.Services;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.Presenters
{
    public class GameplayPresenter : MonoBehaviour
    {
        [SerializeField] private Button buttonExit;
        [SerializeField] private TextMeshPro labelPlayerScore;
        [SerializeField] private TextMeshPro labelEnemyScore;

        private GameplayModel _gameplay;
        private UGUIStateModel _stateMachine;
        private IAudioService _audioService;
        private AudioConfig _audioConfig;
        
        [Inject]
        private void Construct(GameplayModel gameplay, UGUIStateModel stateMachine, IAudioService audioService, AudioConfig audioConfig)
        {
            _gameplay = gameplay;
            _stateMachine = stateMachine;
            
            _audioService = audioService;
            _audioConfig = audioConfig;
        }

        private void Start()
        {
            buttonExit
                .OnClickAsObservable()
                .Subscribe(_ => _stateMachine.State.Value = UGUIState.Menu)
                .AddTo(this);
            
            _stateMachine.State
                .Where(state => state == UGUIState.Gameplay)
                .Subscribe(_ => _gameplay.Reset())
                .AddTo(this);
            
            _gameplay.CurrentPlayerScore
                .SubscribeToText(labelPlayerScore)
                .AddTo(this);
            _gameplay.CurrentEnemyScore
                .SubscribeToText(labelEnemyScore)
                .AddTo(this);

            _gameplay.CurrentPlayerScore
                .Pairwise()
                .Where(score => score.Current > score.Previous)
                .Subscribe(_ =>
                {
                    labelPlayerScore.rectTransform
                        .DOPunchScale(new Vector3(0.25f, 0.25f, 0f), 0.125f)
                        .SetEase(Ease.OutQuint)
                        .SetLink(labelPlayerScore.gameObject)
                        .OnComplete(() => labelPlayerScore.rectTransform.localScale = Vector3.one);
                })
                .AddTo(this);
            _gameplay.CurrentEnemyScore
                .Pairwise()
                .Where(score => score.Current > score.Previous)
                .Subscribe(_ =>
                {
                    _audioService.DuckMusic(0.025f, _audioConfig.musicVolume, 1.2f);
                    
                    labelEnemyScore.rectTransform
                        .DOPunchScale(new Vector3(0.25f, 0.25f, 0f), 0.125f)
                        .SetEase(Ease.OutQuint)
                        .SetLink(labelPlayerScore.gameObject)
                        .OnComplete(() => labelEnemyScore.rectTransform.localScale = Vector3.one);
                })
                .AddTo(this);
        }
    }
}