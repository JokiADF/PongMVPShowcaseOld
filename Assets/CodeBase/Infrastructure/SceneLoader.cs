using System;
using CodeBase.Presenters;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure
{
    public class SceneLoader
    {
        private readonly LoadingPresenter _loadingPresenter;

        public SceneLoader(LoadingPresenter loadingPresenter)
        {
            _loadingPresenter = loadingPresenter;
        }

        public async void Load(string name, Action onLoaded = null)
        {
            _loadingPresenter.Show();
            
            if (SceneManager.GetActiveScene().name == name)
            {
                onLoaded?.Invoke();
                _loadingPresenter.Hide();
                
                return;
            }
            
            await SceneManager.LoadSceneAsync(name, LoadSceneMode.Single)
                .ToUniTask();
            
            onLoaded?.Invoke();
            _loadingPresenter.Hide();
        }
    }
}