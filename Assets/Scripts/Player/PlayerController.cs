using System;
using Controllers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _speed;
        
        [Space]
        [SerializeField] private Joystick _motionJoystick;
        [SerializeField] private ShootingController _shootingController;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Animator _animator;
        
        private Vector3 _motion = default;
        private static readonly int SpeedParam = Animator.StringToHash("Speed");

        private int _upperAvatarLayerIndex;

        private void Awake()
        {
            _upperAvatarLayerIndex = _animator.GetLayerIndex("UpperAvatar");
        }

        private void OnEnable()
        {
            _shootingController.OnStartAiming += SetAiming;
            _shootingController.OnEndAiming += RemoveAiming;
        }

        private void FixedUpdate()
        {
            _motion.Set(_motionJoystick.Horizontal * _speed, 0.0f, _motionJoystick.Vertical * _speed);
            _characterController.Move(_motion);
            
            _animator.SetFloat(SpeedParam, Mathf.Max(Mathf.Abs(_motion.normalized.x), Mathf.Abs(_motion.normalized.z)));
            
            if (_motion != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(_motion.normalized, Vector3.up);
            }
        }

        [EasyButtons.Button]
        private void SetAiming()
        {
            _animator.SetLayerWeight(_upperAvatarLayerIndex, 1.0f);
        }

        [EasyButtons.Button]
        private void RemoveAiming()
        {
            _animator.SetLayerWeight(_upperAvatarLayerIndex, 0.0f);
        }

        private void OnDisable()
        {
            _shootingController.OnStartAiming -= SetAiming;
            _shootingController.OnEndAiming -= RemoveAiming;
        }
        
    }
}
