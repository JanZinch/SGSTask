using System;
using DG.Tweening;
using UnityEngine;

namespace Environment
{
    public class Footstep : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer = null;

        private Action _onFaded = null;

        public void Leave(Vector3 position, Quaternion rotation, Action onFaded)
        {
            transform.position = position;
            transform.rotation = rotation;
            _onFaded = onFaded;
            
            Material material = _renderer.material;

            Color cachedColor = material.color;
            material.color = new Color(cachedColor.r, cachedColor.g, cachedColor.b, 1.0f);
            
            material.DOFade(0.0f, 3.0f).SetEase(Ease.Linear).OnComplete(() =>
            {
                _onFaded?.Invoke();
            });
        }
        
    }
}