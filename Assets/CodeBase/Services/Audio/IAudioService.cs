namespace CodeBase.Services.Audio
{
    public interface IAudioService
    {
        void PlaySfx(string key, float volume);

        void PlayMusic(string key, float volume);

        void StopMusic();

        void DuckMusic(float targetVolume, float originalVolume, float duration);
    }
}
