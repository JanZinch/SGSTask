using Controllers;
using UnityEngine;
using UnityEngine.Events;

namespace Adapters
{
    public class CharacterEventsAdapter : MonoBehaviour
    {
        [SerializeField] private UnityEvent<CharacterSpeedType> _leftFootStep;
        [SerializeField] private UnityEvent<CharacterSpeedType> _rightFootStep;

        public UnityEvent<CharacterSpeedType> LeftFootStep => _leftFootStep;
        public UnityEvent<CharacterSpeedType> RightFootStep => _rightFootStep;
        
        private void OnLeftFootStep(int animationId)
        {
            _leftFootStep?.Invoke((CharacterSpeedType) animationId);
        }
        
        private void OnRightFootStep(int animationId)
        {
            _rightFootStep?.Invoke((CharacterSpeedType) animationId);
        }
        
    }
}