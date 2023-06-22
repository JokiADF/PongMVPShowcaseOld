using CodeBase.Services.AssetManagement;
using SpaceInvaders.Services;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.Helpers
{
    [RequireComponent(typeof(Button))]
    public class ButtonSfx : MonoBehaviour
    {
        private IAudioService _audioService;

        [Inject]
        private void Construct(IAudioService audioService) =>
            _audioService = audioService;
        
        private void Start() =>
            GetComponent<Button>()
                .OnClickAsObservable()
                .Subscribe(_ => 
                    _audioService.PlaySfx(AssetName.Audio.Click, 0.55f))
                .AddTo(this);
    }
}
