namespace CodeBase.Infrastructure.States
{
    public class LobbyLoopState : IState
    {
        private readonly GameStateMachine _gameState;
        private readonly SceneLoader _sceneLoader;

        public LobbyLoopState(GameStateMachine gameState, SceneLoader sceneLoader)
        {
            _gameState = gameState;
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {}

        public void Exit()
        {}
    }
}