using CodeBase.Model;
using CodeBase.Presenters;
using CodeBase.Services.Spawners.Player;
using UnityEngine;
using Zenject;

namespace CodeBase.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindServices();
            BindPlayer();
        }

        private void BindServices()
        {
            Container
                .Bind<InputModel>()
                .AsSingle();
        }

        private void BindPlayer()
        {
            Container
                .Bind<PlayerModel>()
                .AsSingle();
            Container
                .Bind<PlayerSpawner>()
                .AsSingle();
            Container
                .BindFactory<Object, PlayerPresenter, PlayerPresenter.Factory>()
                .FromFactory<PrefabFactory<PlayerPresenter>>();
        }
    }
}
