﻿using System;
using Adapters;
using Controllers;
using Environment;
using Managers;
using UnityEngine;
using UnityEngine.Events;
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
        [SerializeField] private Transform _shotEffectSpawnPoint;
        [SerializeField] private CharacterEventsAdapter _eventsAdapter;
        [SerializeField] private FootstepsTrail _footstepsTrail;
        
        private Vector3 _motion = default;
        private static readonly int SpeedParam = Animator.StringToHash("Speed");
        private static readonly int JumpParam = Animator.StringToHash("Jump");

        private int _upperAvatarLayerIndex;

        private PlayerTarget _currentTarget = null;

        private PlayerState _state = PlayerState.Walking;

        private float _timeBetweenShots = 0.0f;

        private const string JumpBridgeTag = "jump_bridge";

        [EasyButtons.Button]
        public void Put()
        {
            _footstepsTrail.LeaveFootstep(transform.position + Vector3.one * 5.0f, Quaternion.identity, false);
        }

        private void Awake()
        {
            _upperAvatarLayerIndex = _animator.GetLayerIndex("UpperAvatarLayer");
        }


        private void OnEnable()
        {
            _eventsAdapter.LeftFootStep.AddListener(LeaveLeftFootstep);
            _eventsAdapter.RightFootStep.AddListener(LeaveRightFootstep);
        }

        private void LeaveLeftFootstep()
        {
            _footstepsTrail.LeaveFootstep(transform.position, transform.rotation, false);
        }
        
        private void LeaveRightFootstep()
        {
            _footstepsTrail.LeaveFootstep(transform.position, transform.rotation, true);
        }

        private void Update()
        {
            switch (_state)
            {
                case PlayerState.Walking:
                    Walk();
                    break;
                
                case PlayerState.Shooting:
                    Shoot(); 
                    break;

                case PlayerState.Jump:
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
            _motion.Set(_motionJoystick.Horizontal * _speed * Time.deltaTime, 0.0f, _motionJoystick.Vertical * _speed * Time.deltaTime);
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
                StartShooting(target);
            }
            else if (other.TryGetComponent<JumpBridge>(out JumpBridge jumpBridge))
            {
                Jump(jumpBridge);
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
            _currentTarget.OnDestroyed?.RemoveListener(StopShooting);
            _currentTarget = null;
            
            _state = PlayerState.Walking;
            SetAimingAnimation(false);
        }

        private void SetAimingAnimation(bool isActive)
        {
            _animator.SetLayerWeight(_upperAvatarLayerIndex, Convert.ToSingle(isActive));
        }

        private void Jump(JumpBridge jumpBridge)
        {
            _state = PlayerState.Jump;
            _animator.SetTrigger(JumpParam);
            jumpBridge.JumpOver(transform, () =>
            {
                _state = PlayerState.Walking;
            });
        }
        
        private void OnDisable()
        {
            _eventsAdapter.LeftFootStep.RemoveListener(LeaveLeftFootstep);
            _eventsAdapter.RightFootStep.RemoveListener(LeaveRightFootstep);
        }
    }
}
