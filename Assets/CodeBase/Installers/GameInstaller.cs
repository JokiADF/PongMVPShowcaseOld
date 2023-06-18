using CodeBase.Model;
using CodeBase.Presenters;
using CodeBase.Services.Spawners.Ball;
using CodeBase.Services.Spawners.Enemy;
using CodeBase.Services.Spawners.Gameplay;
using CodeBase.Services.Spawners.Input;
using CodeBase.Services.Spawners.Player;
using CodeBase.Services.Spawners.Result;
using UnityEngine;
using Zenject;

namespace CodeBase.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindServices();
            BindScores();
            BindGameplay();
            BindResults();
            
            BindPlayer();
            BindEnemy();
            BindBall();
        }

        private void BindServices()
        {
            Container
                .Bind<InputModel>()
                .AsSingle();
            Container
                .BindInterfacesAndSelfTo<InputSpawner>()
                .AsSingle();
            Container
                .BindFactory<Object, InputPresenter, InputPresenter.Factory>()
                .FromFactory<PrefabFactory<InputPresenter>>();
        }

        private void BindScores()
        {
            Container
                .Bind<ScoresModel>()
                .AsSingle();
        }

        private void BindGameplay()
        {
            Container
                .Bind<GameplayModel>()
                .AsSingle();
            Container
                .BindInterfacesAndSelfTo<GameplaySpawner>()
                .AsSingle();
            Container
                .BindFactory<Object, GameplayPresenter, GameplayPresenter.Factory>()
                .FromFactory<PrefabFactory<GameplayPresenter>>();
        }

        private void BindResults()
        {
            Container
                .BindInterfacesAndSelfTo<ResultsSpawner>()
                .AsSingle();
            Container
                .BindFactory<Object, ResultsPresenter, ResultsPresenter.Factory>()
                .FromFactory<PrefabFactory<ResultsPresenter>>();
        }

        private void BindPlayer()
        {
            Container
                .Bind<PlayerModel>()
                .AsSingle();
            Container
                .BindInterfacesAndSelfTo<PlayerSpawner>()
                .AsSingle();
            Container
                .BindFactory<Object, PlayerPresenter, PlayerPresenter.Factory>()
                .FromFactory<PrefabFactory<PlayerPresenter>>();
        }

        private void BindEnemy()
        {
            Container
                .Bind<EnemyModel>()
                .AsSingle();
            Container
                .BindInterfacesAndSelfTo<EnemySpawner>()
                .AsSingle();
            Container
                .BindFactory<Object, EnemyPresenter, EnemyPresenter.Factory>()
                .FromFactory<PrefabFactory<EnemyPresenter>>();
        }

        private void BindBall()
        {
            Container
                .Bind<BallModel>()
                .AsSingle();
            Container
                .BindInterfacesAndSelfTo<BallSpawner>()
                .AsSingle();
            Container
                .BindFactory<Object, BallPresenter, BallPresenter.Factory>()
                .FromFactory<PrefabFactory<BallPresenter>>();
        }
    }
}
