using System;
using DG.Tweening;
using Extensions;
using UnityEngine;

namespace Environment
{
    public class Footstep : MonoBehaviour
    {
        [SerializeField] private bool _isRight;
        [SerializeField] private float _disappearanceDuration;
        [SerializeField] private Renderer _renderer = null;
        
        public bool IsRight => _isRight;
        private Action _onFaded = null;

        public void Leave(Vector3 position, Quaternion rotation, Vector3 scale, Action onFaded)
        {
            transform.position = position;
            transform.rotation = rotation;
            transform.localScale = scale;
            _onFaded = onFaded;
            
            Material material = _renderer.material;
            
            material.color = material.color.WithAlpha(1.0f);
            material.DOFade(0.0f, _disappearanceDuration).SetEase(Ease.Linear).OnComplete(() =>
            {
                _onFaded?.Invoke();
            });
        }
        
    }
}