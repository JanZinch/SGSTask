using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

namespace Environment
{
    public class DestructibleObject : MonoBehaviour
    {
        [SerializeField] private int _maxHealth = 100;
        private int _health = default;
        
        public double Health => _health;
        public event Action OnDeath = null;
        public event Action<int> OnTakeDamage = null;
        public event Action<int> OnHealthUpdate = null;

        private void Awake()
        {
            _health = _maxHealth;
        }

        public void MakeDamage(int damage)
        {
            _health = Mathf.Clamp(_health - damage, 0, _maxHealth);

            OnTakeDamage?.Invoke(damage);
            OnHealthUpdate?.Invoke(_health);
            
            if (_health <= 0)
            {
                OnDeath?.Invoke();
            }
        }
        
    }
}