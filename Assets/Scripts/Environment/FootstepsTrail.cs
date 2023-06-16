using System;
using Extensions;
using UnityEngine;
using UnityEngine.Pool;

namespace Environment
{
    public class FootstepsTrail : MonoBehaviour
    {
        [SerializeField] private Footstep _footstepOriginal;
        private IObjectPool<Footstep> _footstepsPool = null;

        public void LeaveFootstep(Vector3 position, Quaternion rotation, bool isRight)
        {
            Footstep footstep = _footstepsPool.Get();

            Vector3 sourceFootstepScale = footstep.transform.localScale;
            Vector3 modifiedFootstepScale = footstep.IsRight != isRight
                ? sourceFootstepScale.WithX(Mathf.Abs(sourceFootstepScale.x) * -1.0f)
                : sourceFootstepScale.WithX(Mathf.Abs(sourceFootstepScale.x));

            footstep.Leave(position, rotation, modifiedFootstepScale, () =>
            {
                _footstepsPool.Release(footstep);
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