using UnityEngine;
using UnityEngine.Events;

namespace Environment
{
    public class DestructibleObject : MonoBehaviour
    {
        [SerializeField] private int _maxHealth = 100;
        [SerializeField] private UnityEvent<int> _onHealthUpdate;
        [SerializeField] private UnityEvent _onDeath;
        
        private int _health = default;
        
        public double Health => _health;
        public UnityEvent<int> OnHealthUpdate => _onHealthUpdate;
        public UnityEvent OnDeath => _onDeath;
        
        private void Awake()
        {
            _health = _maxHealth;
        }

        public void MakeDamage(int damage)
        {
            _health = Mathf.Clamp(_health - damage, 0, _maxHealth);
            
            _onHealthUpdate?.Invoke(_health);
            
            if (_health <= 0)
            {
                _onDeath?.Invoke();
                
                _onHealthUpdate?.RemoveAllListeners();
                _onDeath?.RemoveAllListeners();
            }
        }
        
    }
}