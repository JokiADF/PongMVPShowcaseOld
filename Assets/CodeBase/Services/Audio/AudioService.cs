using System;
using CodeBase.Services.AssetManagement;
using DG.Tweening;
using SpaceInvaders.Services;
using UnityEngine;

namespace CodeBase.Services.Audio
{
    [Serializable]
    public class AudioConfig
    {
        [Range(0f, 1f)]
        public float musicVolume = 0.5f;
    }

    public class AudioService : IAudioService
    {
        private readonly IAssetService _assetService;

        private Transform _cameraTransform;
        private AudioSource _music;
        private Tween _tween;

        public AudioService(IAssetService assetService)
        {
            _assetService = assetService;
        }

        public void PlaySfx(string key, float volume)
        {
            var clip = _assetService.Get<AudioClip>(key);

            if (clip == null)
            {
                Debug.LogWarning($"Couldn't find '{key}' AudioClip.");
                
                return;
            }

            if (_cameraTransform == null) 
                _cameraTransform = Camera.main.transform;

            AudioSource.PlayClipAtPoint(clip, _cameraTransform.position, volume);
        }

        public void PlayMusic(string key, float volume)
        {
            var clip = _assetService.Get<AudioClip>(key);

            if (clip == null)
            {
                Debug.LogWarning($"Couldn't find '{key}' AudioClip.");
                
                return;
            }

            if (_music == null)
            {
                var go = new GameObject("Music");
                _music = go.AddComponent<AudioSource>();
                _music.spatialBlend = 0;
                _music.volume = 0;
                _music.loop = true;
            }

            _tween?.Kill();
            _tween = _music.DOFade(volume, 2f)
                .SetEase(Ease.InQuad)
                .OnStart(() =>
                {
                    _music.clip = clip;
                    _music.volume = 0f;
                    _music.Play();
                })
                .OnComplete(() =>
                {
                    _music.volume = volume;
                });
        }

        public void StopMusic()
        {
            if (_music == null)
                return;

            _tween?.Kill();
            _tween = _music.DOFade(0f, 2f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    _music.volume = 0f;
                    _music.Stop();
                });
        }

        public void DuckMusic(float targetVolume, float originalVolume, float duration)
        {
            if (_music == null)
                return;

            _tween?.Kill();
            _tween = _music.DOFade(targetVolume, duration)
                .SetEase(Ease.OutQuad)
                .SetLoops(2, LoopType.Yoyo)
                .OnComplete(() =>
                {
                    _music.volume = originalVolume;
                });
        }
    }
}
