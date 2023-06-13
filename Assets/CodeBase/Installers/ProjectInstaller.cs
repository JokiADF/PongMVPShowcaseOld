using CodeBase.Infrastructure;
using CodeBase.Infrastructure.States;
using CodeBase.Presenters;
using CodeBase.Services.AssetManagement;
using UnityEngine;
using Zenject;

namespace CodeBase.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private LoadingPresenter prefab;
        
        public override void InstallBindings()
        {
            BindGame();
            BindServices();
        }

        private void BindGame()
        {
            var loadingPresenter = Instantiate(prefab);
            var game = new Game(loadingPresenter);
            Container
                .Bind<Game>()
                .FromInstance(game)
                .AsSingle();
            Container
                .Bind<GameStateMachine>()
                .FromInstance(game.StateMachine)
                .AsSingle();
            game.StateMachine.Enter<BootstrapState>();
        }

        private void BindServices()
        {
            Container
                .BindInterfacesAndSelfTo<AddressablesService>()
                .AsSingle();
        }
    }
}