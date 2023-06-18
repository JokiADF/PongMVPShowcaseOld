using CodeBase.Services.AssetManagement;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class LoadLobbyState : IState
    {
        private const string Lobby = "Lobby";
        
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IAssetService _assetService;
        
        public LoadLobbyState(GameStateMachine stateMachine, SceneLoader sceneLoader, IAssetService assetService)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _assetService = assetService;
        }
        
        public void Enter()
        {
            // await LoadAssetsAsync();
            
            _sceneLoader.Load(Lobby, EnterLobbyLoop);
        }

        private void EnterLobbyLoop() => 
            _stateMachine.Enter<LobbyLoopState>();

        private async UniTask LoadAssetsAsync()
        {
            // await null;
        }

        public void Exit()
        {
            throw new System.NotImplementedException();
        }
    }
}