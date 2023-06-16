using CodeBase.Model;
using CodeBase.Presenters;
using CodeBase.Services.Spawners.Ball;
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
            BindBall();
        }

        private void BindServices()
        {
            Container
                .Bind<InputModel>()
                .AsSingle();
        }

        private void BindPlayer()
        {
            Container.Bind<PlayerModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerSpawner>().AsSingle();
            Container.BindFactory<Object, PlayerPresenter, PlayerPresenter.Factory>().FromFactory<PrefabFactory<PlayerPresenter>>();
        }

        private void BindBall()
        {
            Container.Bind<BallModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<BallSpawner>().AsSingle();
            Container.BindFactory<Object, BallPresenter, BallPresenter.Factory>().FromFactory<PrefabFactory<BallPresenter>>();
        }
    }
}
