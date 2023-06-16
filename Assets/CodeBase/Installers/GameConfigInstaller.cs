using CodeBase.Model;
using UnityEngine;
using Zenject;

namespace CodeBase.Installers
{
    [CreateAssetMenu(fileName = "GameConfigInstaller", menuName = "Installers/GameConfigInstaller")]
    public class GameConfigInstaller : ScriptableObjectInstaller<GameConfigInstaller>
    {
        [SerializeField] private PlayerConfig player;
        [SerializeField] private EnemyConfig enemy;
        [SerializeField] private BallConfig ball;
        [SerializeField] private LevelConfig level;

        public override void InstallBindings()
        {
            Container.BindInstances(player);
            Container.BindInstances(enemy);
            Container.BindInstances(ball);
            Container.BindInstances(level);
        }
    }
}
