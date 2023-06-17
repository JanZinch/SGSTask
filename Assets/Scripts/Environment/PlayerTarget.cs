using Managers;
using UnityEngine;
using UnityEngine.Events;

namespace Environment
{
    public class PlayerTarget : MonoBehaviour
    {
        [SerializeField] private DestructibleObject _destructible;
        [SerializeField] private UnityEvent _onDestroyed;

        public UnityEvent OnDestroyed => _onDestroyed;
        
        private void OnEnable()
        {
            _destructible.OnDeath.AddListener(SelfDestroy);
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
            _destructible.OnDeath.RemoveListener(SelfDestroy);
        }

        private void OnDestroy()
        {
            _onDestroyed?.Invoke();
            _onDestroyed?.RemoveAllListeners();
        }
    }
}