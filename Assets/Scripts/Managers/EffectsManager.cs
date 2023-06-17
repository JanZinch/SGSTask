using System;
using UnityEngine;

namespace Managers
{
    public class EffectsManager : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _shotEffectOriginal;
        [SerializeField] private ParticleSystem _explosionEffectOriginal;

        public static EffectsManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
                Instance = null;
            }
            
            Instance = this;
        }

        public void MakeShot(Vector3 position)
        {
            Instantiate(_shotEffectOriginal, position, Quaternion.identity);
        }
        
        public void MakeExplosion(Vector3 position)
        {
            Instantiate(_explosionEffectOriginal, position, Quaternion.identity);
        }
    }
}