using UnityEngine;

namespace CodeBase.Helpers
{
    public class ButtonAxis : MonoBehaviour
    {
        [SerializeField] private ButtonTouch buttonNegative;
        [SerializeField] private ButtonTouch buttonPositive;
        
        public float Axis { get; private set; }

        private void Update() => 
            Axis = buttonNegative.IsPressed ? -1f : buttonPositive.IsPressed ? 1f : 0f;
    }
}