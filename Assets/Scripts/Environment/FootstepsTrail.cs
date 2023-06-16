using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Environment
{
    public class FootstepsTrail : MonoBehaviour
    {
        [SerializeField] private Footstep _footstepOriginal;

        private IObjectPool<Footstep> _footstepsPool = null;

        public void LeaveFootstep(Vector3 position, Quaternion rotation)
        {
            Footstep leavedFootstep = _footstepsPool.Get();
            
            leavedFootstep.Leave(position, rotation, () =>
            {
                _footstepsPool.Release(leavedFootstep);
            });
        }

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