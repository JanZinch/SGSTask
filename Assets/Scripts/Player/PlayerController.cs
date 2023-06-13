using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _speed;
        
        [Space]
        [SerializeField] private Joystick _joystick;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Animator _animator;
        
        private Vector3 _motion = default;
        private static readonly int SpeedParam = Animator.StringToHash("Speed");

        private void FixedUpdate()
        {
            _motion.Set(_joystick.Horizontal * _speed, 0.0f, _joystick.Vertical * _speed);
            _characterController.Move(_motion);
            
            _animator.SetFloat(SpeedParam, Mathf.Max(Mathf.Abs(_motion.normalized.x), Mathf.Abs(_motion.normalized.z)));

            //Vector3 vector = Vector3.up * _joystick.Horizontal + Vector3.left * _joystick.Vertical;

            if (_motion != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(_motion.normalized, Vector3.up);
            }
        }
    }
}