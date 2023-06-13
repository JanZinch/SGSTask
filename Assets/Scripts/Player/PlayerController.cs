using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _speed;
        
        [Space]
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Joystick _joystick;

        private Vector3 _motion = default;

        private void FixedUpdate()
        {
            _motion.Set(_joystick.Horizontal * _speed, 0.0f, _joystick.Vertical * _speed);
            _characterController.Move(_motion);
        }
    }
}