using UnityEngine;
using UnityEngine.Events;

namespace Adapters
{
    public class CharacterEventsAdapter : MonoBehaviour
    {
        [SerializeField] private UnityEvent _leftFootStep;
        [SerializeField] private UnityEvent _rightFootStep;

        public UnityEvent LeftFootStep => _leftFootStep;
        public UnityEvent RightFootStep => _rightFootStep;
        
        private void OnLeftFootStep()
        {
            Debug.Log("On left footstep");
            _leftFootStep?.Invoke();
        }
        
        private void OnRightFootStep()
        {
            Debug.Log("On right footstep");
            _rightFootStep?.Invoke();
        }
        
    }
}