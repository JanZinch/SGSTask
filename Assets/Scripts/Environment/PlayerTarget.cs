using System;
using DG.Tweening;
using Managers;
using UnityEngine;
using UnityEngine.Events;

namespace Environment
{
    public class PlayerTarget : MonoBehaviour
    {
        [SerializeField] private DestructibleObject _destructible;
        [SerializeField] private ParticleSystem _explosionEffectOriginal;

        [SerializeField] private UnityEvent _onDestroyed;

        public UnityEvent OnDestroyed => _onDestroyed;
        
        private void OnEnable()
        {
            _destructible.OnDeath += SelfDestroy;
        }

        public void Fire(int damage)
        {
            _destructible.MakeDamage(damage);
        }

        private void SelfDestroy()
        {
            EffectsManager.Instance.MakeExplosion(transform.position);
            Destroy(gameObject);
        }
        
        private void OnDisable()
        {
            _destructible.OnDeath -= SelfDestroy;
        }

        private void OnDestroy()
        {
            _onDestroyed?.Invoke();
            _onDestroyed?.RemoveAllListeners();
            
            
            
            /*DOVirtual.DelayedCall(explosionEffect.main.duration, () =>
            {
                Destroy(explosionEffect.gameObject);
                
            }).SetLink(gameObject);*/
        }
    }
}