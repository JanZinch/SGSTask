using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Environment
{
    public class PlayerTarget : MonoBehaviour
    {
        [SerializeField] private DestructibleObject _destructible;
        [SerializeField] private ParticleSystem _explosionEffectOriginal;
        
        public UnityEvent OnDestroyed = null;
        
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
            Destroy(gameObject);
        }
        
        private void OnDisable()
        {
            _destructible.OnDeath -= SelfDestroy;
        }

        private void OnDestroy()
        {
            OnDestroyed?.Invoke();
            OnDestroyed?.RemoveAllListeners();
            
            ParticleSystem explosionEffect = Instantiate(_explosionEffectOriginal, transform.position, Quaternion.identity);
            
            /*DOVirtual.DelayedCall(explosionEffect.main.duration, () =>
            {
                Destroy(explosionEffect.gameObject);
                
            }).SetLink(gameObject);*/
        }
    }
}