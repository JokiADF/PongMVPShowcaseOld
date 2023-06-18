using System;
using System.Collections.Generic;
using CodeBase.Services.AssetManagement;
using CodeBase.Services.Spawners.Result;
using UniRx;

namespace CodeBase.Infrastructure.States
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public ReactiveProperty<Type> ActiveStateType => new (_activeState.GetType());

        public GameStateMachine(SceneLoader sceneLoader, IAssetService assetService)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader),
                [typeof(LoadLobbyState)] = new LoadLobbyState(this, sceneLoader, assetService),
                [typeof(LobbyLoopState)] = new LobbyLoopState(this, sceneLoader),
                [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, assetService),
                [typeof(GameLoopState)] = new GameLoopState(),
                [typeof(ResultLoopState)] = new ResultLoopState()
            };
        }
        
        public void Enter<TState>() where TState : class, IState
        {
            var state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            var state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();
            
            var state = GetState<TState>();
            _activeState = state;
            
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState => 
            _states[typeof(TState)] as TState;
    }
}
