using System;
using DG.Tweening;
using UnityEngine;

namespace Environment
{
    public class JumpBridge : MonoBehaviour
    {
        [SerializeField] private float _distance;
        [SerializeField] private AnimationClip _jumpAnimationClip;
        
        public void JumpOver(Transform jumper, Action onJumped = null)
        {
            jumper.DOLocalMoveZ(jumper.localPosition.z + _distance * DetermineSide(jumper), _jumpAnimationClip.length)
                .SetEase(Ease.OutCirc).SetLink(gameObject).OnComplete(() => onJumped?.Invoke());
        }

        private int DetermineSide(Transform jumper) 
        {
            return -1 * Math.Sign(transform.InverseTransformPoint(jumper.position).z);
        }

    }
}