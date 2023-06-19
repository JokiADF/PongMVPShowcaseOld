using CodeBase.Infrastructure;
using CodeBase.Infrastructure.States;
using CodeBase.Model;
using CodeBase.Presenters;
using CodeBase.Services.AssetManagement;
using CodeBase.Services.Storage;
using UnityEngine;
using Zenject;

namespace CodeBase.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private LoadingPresenter prefab;
        
        public override void InstallBindings()
        {
            BindServices();
            BindSceneLoader();
            BindGame();
        }

        private void BindServices()
        {
            Container
                .BindInterfacesAndSelfTo<AddressablesService>()
                .AsSingle();
            Container
                .BindInterfacesAndSelfTo<StorageService>()
                .AsSingle();
        }

        private void BindSceneLoader()
        {
            var loadingPresenter = Instantiate(prefab);
            Container
                .Bind<SceneLoader>()
                .FromInstance(new SceneLoader(loadingPresenter))
                .AsSingle();
        }

        private void BindGame()
        {
            Container
                .Bind<Game>()
                .AsSingle();
            Container
                .Bind<GameStateMachine>()
                .AsSingle();
        }
    }
}