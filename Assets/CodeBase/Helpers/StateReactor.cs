using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace CodeBase.Helpers
{
    public abstract class StateReactor<T> : MonoBehaviour where T : Enum
    {
        [Tooltip("List of states in which this object should be visible.")]
        [SerializeField] private List<T> visibleInStates;

        protected abstract StateModel<T> Model { get; }

        private void Start()
        {
            Model.State.Subscribe(state => 
                gameObject.SetActive(IsVisible(state))).AddTo(this);
        }

        private bool IsVisible(T state)
        {
            if (state == null)
            {
                return false;
            }

            return visibleInStates.Contains(state);
        }
    }
}
