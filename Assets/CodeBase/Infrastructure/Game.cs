using CodeBase.Infrastructure.States;
using CodeBase.Presenters;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class Game
    {
        public readonly GameStateMachine StateMachine;

        public Game(GameStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }
        
    }
}