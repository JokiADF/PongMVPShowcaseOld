using CodeBase.Services.Spawners.Player;

namespace CodeBase.Infrastructure.States
{
    public class GameLoopState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IPlayerSpawner _playerSpawner;

        public GameLoopState(GameStateMachine stateMachine, SceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {}

        public void Exit()
        {}
    }
}