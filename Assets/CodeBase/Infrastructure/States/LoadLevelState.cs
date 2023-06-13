﻿using CodeBase.Services.AssetManagement;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IState
    {
        private const string Game = "Game";
        
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IAssetService _assetService;

        public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, IAssetService assetService)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _assetService = assetService;
        }

        public async void Enter()
        {
            await LoadAssetsAsync();
            
            _sceneLoader.Load(Game, EnterGameLoop);
        }

        private void EnterGameLoop() => 
            _stateMachine.Enter<GameLoopState>();

        private async UniTask LoadAssetsAsync()
        {
            await _assetService.Load<GameObject>(AssetName.Objects.Player);
        }

        public void Exit()
        {}
    }
}