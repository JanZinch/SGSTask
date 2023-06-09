﻿using System;
using DG.Tweening;
using UnityEngine;

namespace Environment
{
    public class Gap : MonoBehaviour
    {
        [SerializeField] private float _distance;
        [SerializeField] private AnimationClip _jumpAnimationClip;
        [SerializeField] private Collider _safetyCollider;
        
        public void JumpOver(Transform jumper, Action onJumped = null)
        {
            _safetyCollider.enabled = false;

            float jumperTargetZ = jumper.localPosition.z + _distance * DetermineSide(jumper);
            
            jumper.DOLocalMoveZ(jumperTargetZ, _jumpAnimationClip.length)
                .SetEase(Ease.OutSine).SetLink(gameObject).OnComplete(() =>
                {
                    _safetyCollider.enabled = true;
                    onJumped?.Invoke();
                });
        }
        
        private int DetermineSide(Transform jumper) 
        {
            return -1 * Math.Sign(transform.InverseTransformPoint(jumper.position).z);
        }
    }
}