using System;
using UnityEngine;
using UnityEngine.Events;

namespace Environment
{
    public class PlayerTarget : MonoBehaviour
    {
        [SerializeField] private DestructibleObject _destructible = null;

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
        }
    }
}