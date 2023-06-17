using System;
using DG.Tweening;
using Extensions;
using UnityEngine;

namespace Environment
{
    public class Footstep : MonoBehaviour
    {
        [SerializeField] private bool _isRight;
        [SerializeField] private float _disappearanceDuration = 10.0f;
        [SerializeField] private Ease _disappearanceEase = Ease.InCirc;
        [SerializeField] private Renderer _renderer;
        
        public bool IsRight => _isRight;
        private Action _onFaded = null;

        public void Leave(Vector3 position, Quaternion rotation, Vector3 scale, Action onFaded)
        {
            Transform selfTransform = transform;
            selfTransform.position = position;
            selfTransform.rotation = rotation;
            selfTransform.localScale = scale;
            _onFaded = onFaded;
            
            Material material = _renderer.material;
            
            material.color = material.color.WithAlpha(1.0f);
            material.DOFade(0.0f, _disappearanceDuration).SetEase(_disappearanceEase).OnComplete(() =>
            {
                _onFaded?.Invoke();
            });
        }
        
    }
}