using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Environment
{
    public class FootstepFactory : MonoBehaviour
    {
        [SerializeField] private Footstep _footstepOriginal;

        private IObjectPool<Footstep> _footstepsPool = null;

        private void Awake()
        {
            _footstepsPool = new ObjectPool<Footstep>(CreateFootstep, OnGetFromPool, OnReturnToPool, DestroyFootstep,
                true, 10, 100);
        }

        private Footstep CreateFootstep()
        {
            return Instantiate<Footstep>(_footstepOriginal);
        }

        private void OnGetFromPool(Footstep footstep)
        {
            footstep.gameObject.SetActive(true);
        }
        
        private void OnReturnToPool(Footstep footstep)
        {
            footstep.gameObject.SetActive(false);
        }

        private void DestroyFootstep(Footstep footstep)
        {
            Destroy(footstep.gameObject);
        }

    }
}