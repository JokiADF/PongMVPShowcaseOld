using CodeBase.Infrastructure.States;
using CodeBase.Presenters;

namespace CodeBase.Infrastructure
{
    public class Game
    {
        public readonly GameStateMachine StateMachine;

        public Game(LoadingPresenter loadingPresenter)
        {
            StateMachine = new GameStateMachine(new SceneLoader(loadingPresenter));
        }
    }
}