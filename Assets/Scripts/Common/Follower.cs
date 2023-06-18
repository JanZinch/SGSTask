using UnityEngine;

namespace Common
{
    public class Follower : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _speed;

        private Vector3 _cachedPosition;
        private Vector3 _delta;

        private void Start()
        {
            _delta = _target.position - transform.position;
        }

        private void Update()
        {
            float interpolation = _speed * Time.deltaTime;
            Vector3 targetPosition = _target.position;
            
            _cachedPosition = transform.position;
            _cachedPosition.x = Mathf.Lerp(_cachedPosition.x, targetPosition.x - _delta.x, interpolation);
            _cachedPosition.z = Mathf.Lerp(_cachedPosition.z, targetPosition.z - _delta.z, interpolation);
            transform.position = _cachedPosition;
        }
    }
}