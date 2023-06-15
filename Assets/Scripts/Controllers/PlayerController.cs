using System;
using Controllers;
using DG.Tweening;
using Environment;
using TreeEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _speed = 0.1f;
        [SerializeField] private float _shootingCooldown = 1.0f;
        
        [Space]
        [SerializeField] private Joystick _motionJoystick;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Animator _animator;
        [SerializeField] private ParticleSystem _shotEffectOriginal;
        [SerializeField] private Transform _shotEffectSpawnPoint;
        
        private Vector3 _motion = default;
        private static readonly int SpeedParam = Animator.StringToHash("Speed");

        private int _upperAvatarLayerIndex;

        private PlayerTarget _currentTarget = null;

        private PlayerState _state = PlayerState.Walking;

        private float _timeBetweenShots = 0.0f;
        
        private void Awake()
        {
            _upperAvatarLayerIndex = _animator.GetLayerIndex("UpperAvatar");
        }
        

        private void FixedUpdate()
        {
            switch (_state)
            {
                case PlayerState.Walking:
                    Walk();
                    break;
                
                case PlayerState.Shooting:
                    Shoot(); 
                    break;

                default:
                    break;
            }
            
            
        }

        private void Walk()
        {
            MoveByJoystick();
            
            if (_motion != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(_motion.normalized, Vector3.up);
            }
        }

        private void MoveByJoystick()
        {
            _motion.Set(_motionJoystick.Horizontal * _speed, 0.0f, _motionJoystick.Vertical * _speed);
            _characterController.Move(_motion);
            
            _animator.SetFloat(SpeedParam, Mathf.Max(Mathf.Abs(_motion.normalized.x), Mathf.Abs(_motion.normalized.z)));
        }

        private void Shoot()
        {
            MoveByJoystick();
            
            transform.rotation = Quaternion.LookRotation(GetDirectionTo(_currentTarget.transform), Vector3.up);
            
            if (_timeBetweenShots > _shootingCooldown)
            {
                _currentTarget.Fire(35);
                Debug.Log("Fire!");
                _timeBetweenShots = 0.0f;

                ParticleSystem shotEffect = Instantiate<ParticleSystem>(_shotEffectOriginal, _shotEffectSpawnPoint.transform.position, Quaternion.identity);

                DOVirtual.DelayedCall(shotEffect.main.duration, () =>
                {
                    Destroy(shotEffect.gameObject);
                }).SetLink(gameObject);
            }
            else
            {
                _timeBetweenShots += Time.fixedDeltaTime;
            }
        }

        private Vector3 GetDirectionTo(Transform target)
        {
            return target.transform.position - transform.position;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerTarget>(out PlayerTarget target))
            {
                StartShooting(target);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<PlayerTarget>(out PlayerTarget target))
            {
                if (_currentTarget == target)
                {
                    StopShooting();
                }
            }
        }

        private void StartShooting(PlayerTarget target)
        {
            _currentTarget = target;
            _currentTarget.OnDestroyed.AddListener(StopShooting);
            
            _state = PlayerState.Shooting;
            SetAimingAnimation(true);
        }
        
        private void StopShooting()
        {
            _currentTarget.OnDestroyed.RemoveListener(StopShooting);
            _currentTarget = null;
            
            _state = PlayerState.Walking;
            SetAimingAnimation(false);
        }

        private void SetAimingAnimation(bool isActive)
        {
            _animator.SetLayerWeight(_upperAvatarLayerIndex, Convert.ToSingle(isActive));
        }
        
    }
}
