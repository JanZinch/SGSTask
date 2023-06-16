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
            ParticleSystem explosionEffect = Instantiate(_explosionEffectOriginal, transform.position, Quaternion.identity);
            
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