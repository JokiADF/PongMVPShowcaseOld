using CodeBase.Model;
using CodeBase.Presenters;
using CodeBase.Services.AssetManagement;
using CodeBase.Services.Audio;
using CodeBase.Services.CameraShaker;
using CodeBase.Services.Spawners.Ball;
using CodeBase.Services.Spawners.Enemy;
using CodeBase.Services.Spawners.Explosion;
using CodeBase.Services.Spawners.Input;
using CodeBase.Services.Spawners.Player;
using CodeBase.Services.Storage;
using UnityEngine;
using Zenject;

namespace CodeBase.Installers
{
    public class GameInstaller : MonoInstaller
    {
        private IAssetService _assetService;

        [Inject]
        private void Construct(IAssetService assetService) =>
            _assetService = assetService;
        
        public override void InstallBindings()
        {
            BindUGUI();
            BindServices();
            BindInput();
            
            BindPlayer();
            BindEnemy();
            BindBall();
            
            BindExplosions();
        }

        private void BindServices()
        {
            Container
                .BindInterfacesAndSelfTo<AudioService>()
                .AsSingle();
            Container
                .BindInterfacesAndSelfTo<StorageService>()
                .AsSingle();
            Container
                .BindInterfacesAndSelfTo<CameraShaker>()
                .AsSingle();
        }

        private void BindUGUI()
        {
            Container
                .Bind<UGUIStateModel>()
                .AsSingle();
            Container
                .Bind<GameplayModel>()
                .AsSingle();
            Container
                .Bind<ScoresModel>()
                .AsSingle();
        }

        private void BindInput()
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
        
        private void BindExplosions()
        {
            Container
                .BindInterfacesAndSelfTo<ExplosionSpawner>()
                .AsSingle();
            Container.BindFactory<Vector3, ExplosionPresenter, ExplosionPresenter.Factory>()
                .FromMonoPoolableMemoryPool( x => 
                    x.WithInitialSize(4).FromComponentInNewPrefab(_assetService.Get<GameObject>(AssetName.Objects.Explosion)).UnderTransformGroup("ExplosionPool"));
        }
    }
}
