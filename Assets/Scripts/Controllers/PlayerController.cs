using System;
using System.Collections.Generic;
using Adapters;
using Controllers;
using Environment;
using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _maxSpeed = 3.0f;
        [SerializeField] private int _damage = 35;
        [SerializeField] private float _shootingCooldown = 1.0f;
        
        [Space]
        [SerializeField] private Joystick _motionJoystick;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _shotEffectSpawnPoint;
        [SerializeField] private CharacterEventsAdapter _eventsAdapter;
        [SerializeField] private FootstepsTrail _footstepsTrail;

        private static readonly Vector3 PlayerRotationMask = new Vector3(0.0f, 1.0f, 0.0f);
        private static readonly Vector3 LeftStepOffset = new Vector3(-0.075f, 0.0f, 0.0f);
        private static readonly Vector3 RightStepOffset = new Vector3(0.075f, 0.0f, 0.0f);
        
        private static readonly int SpeedParam = Animator.StringToHash("Speed");
        private static readonly int JumpParam = Animator.StringToHash("Jump");
        private int _upperAvatarLayerIndex;
        private int _legsFixLayerIndex;
        
        private readonly LinkedList<PlayerTarget> _currentTargets = new LinkedList<PlayerTarget>();

        private Vector3 _motion = default;
        private Vector3 _cachedSelfPosition = default;
        private float _timeBetweenShots;
        
        private PlayerState _state = PlayerState.Walking;

        private void Awake()
        {
            _upperAvatarLayerIndex = _animator.GetLayerIndex("UpperAvatarLayer");
            _legsFixLayerIndex = _animator.GetLayerIndex("LegsFixLayer");
        }
        
        private void OnEnable()
        {
            _eventsAdapter.LeftFootStep.AddListener(LeaveLeftFootstep);
            _eventsAdapter.RightFootStep.AddListener(LeaveRightFootstep);
        }
        
        private void Update()
        {
            switch (_state)
            {
                case PlayerState.Walking:
                    Walk();
                    break;
                
                case PlayerState.Shooting:
                    ShootIfReady(); 
                    break;

                case PlayerState.Jump:
                case PlayerState.None:
                default:
                    break;
            }

            _cachedSelfPosition = transform.position;
            _cachedSelfPosition.y = 0.05f;
            transform.position = _cachedSelfPosition;
        }

        private void MoveByJoystick()
        {
            _motion.Set(
                _motionJoystick.Horizontal * _maxSpeed * Time.deltaTime, 
                0.0f, 
                _motionJoystick.Vertical * _maxSpeed * Time.deltaTime);
            
            _characterController.Move(_motion);
            
            Debug.Log("x: " + _motionJoystick.Horizontal + " y: " + _motionJoystick.Vertical);
            
            //_animator.SetFloat(SpeedParam, Mathf.Max(Mathf.Abs(_motion.normalized.x), Mathf.Abs(_motion.normalized.z)));
            _animator.SetFloat(SpeedParam, _motionJoystick.Direction.magnitude);
            
            //_animator.SetFloat(SpeedParam, (Vector3.forward * _motionJoystick.Vertical + Vector3.right * _motionJoystick.Horizontal).magnitude);
        }

        private void Walk()
        {
            MoveByJoystick();
            
            if (_motion != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(_motion.normalized, Vector3.up);
                transform.eulerAngles = Vector3.Scale(lookRotation.eulerAngles, PlayerRotationMask);
            }
        }
        
        private void ShootIfReady()
        {
            MoveByJoystick();
            
            Quaternion lookRotation = Quaternion.LookRotation(
                GetDirectionTo(_currentTargets.First.Value.transform), Vector3.up);
            transform.eulerAngles = Vector3.Scale(lookRotation.eulerAngles, PlayerRotationMask);
            
            if (_timeBetweenShots > _shootingCooldown)
            {
                _currentTargets.First.Value.Fire(_damage);
                _timeBetweenShots = 0.0f;

                EffectsManager.Instance.MakeShot(_shotEffectSpawnPoint.transform.position);
            }
            else
            {
                _timeBetweenShots += Time.deltaTime;
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
                AddShootingTarget(target);
            }
            else if (other.TryGetComponent<Gap>(out Gap gap))
            {
                JumpOver(gap);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<PlayerTarget>(out PlayerTarget target))
            {
                if (_currentTargets.Contains(target))
                {
                    RemoveShootingTarget(target);
                }
            }
        }

        private void StartShooting()
        {
            _state = PlayerState.Shooting;
            ActivateAimingAnimation(true);
        }
        
        private void AddShootingTarget(PlayerTarget target)
        {
            if (_currentTargets.Count == 0)
            {
                StartShooting();
            }

            _currentTargets.AddLast(target);
            target.OnDestroyed.AddListener(NextShootingTarget);
        }
        
        private void NextShootingTarget()
        {
            PlayerTarget target = _currentTargets.First.Value;
            _currentTargets.RemoveFirst();
            target.OnDestroyed.RemoveListener(NextShootingTarget);

            if (_currentTargets.Count == 0)
            {
                StopShooting();
            }
            else
            {
                RestartAimingAnimation();
            }
        }
        
        private void RemoveShootingTarget(PlayerTarget target)
        {
            if (_currentTargets.First.Value == target)
            {
                NextShootingTarget();
            }
            else
            {
                _currentTargets.Remove(target);
                target.OnDestroyed.RemoveListener(NextShootingTarget);
            }
        }

        private void StopShooting()
        {
            _state = PlayerState.Walking;
            ActivateAimingAnimation(false);
        }

        private void ActivateAimingAnimation(bool isActive)
        {
            _animator.SetLayerWeight(_upperAvatarLayerIndex, Convert.ToSingle(isActive));
            _animator.SetLayerWeight(_legsFixLayerIndex, isActive ? 0.3f : 0.0f);

            if (isActive)
            {
                RestartAimingAnimation();
            }
        }

        private void RestartAimingAnimation()
        {
            _animator.Play(_animator.GetCurrentAnimatorStateInfo(_upperAvatarLayerIndex).fullPathHash, _upperAvatarLayerIndex, 0.0f);
            _animator.Play(_animator.GetCurrentAnimatorStateInfo(_legsFixLayerIndex).fullPathHash, _legsFixLayerIndex, 0.0f);
            
            _timeBetweenShots = _shootingCooldown * 0.75f;
        }

        private void JumpOver(Gap gap)
        {
            _state = PlayerState.Jump;
            _animator.SetTrigger(JumpParam);
            gap.JumpOver(transform, () =>
            {
                _state = _currentTargets.Count > 0 ? PlayerState.Shooting : PlayerState.Walking;
            });
        }
        
        private void LeaveLeftFootstep()
        {
            Transform selfTransform = transform;
            _footstepsTrail.LeaveFootstep(selfTransform.TransformPoint(LeftStepOffset),
                selfTransform.rotation, false);
        }
        
        private void LeaveRightFootstep()
        {
            Transform selfTransform = transform;
            _footstepsTrail.LeaveFootstep(selfTransform.TransformPoint(RightStepOffset),
                selfTransform.rotation, true);
        }
        
        private void OnDisable()
        {
            _eventsAdapter.LeftFootStep.RemoveListener(LeaveLeftFootstep);
            _eventsAdapter.RightFootStep.RemoveListener(LeaveRightFootstep);
        }
    }
}
