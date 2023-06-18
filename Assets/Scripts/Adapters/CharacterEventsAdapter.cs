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
            Debug.Log("On left footstep");
            _leftFootStep?.Invoke((CharacterSpeedType) animationId);
        }
        
        private void OnRightFootStep(int animationId)
        {
            Debug.Log("On right footstep");
            _rightFootStep?.Invoke((CharacterSpeedType) animationId);
        }
        
    }
}