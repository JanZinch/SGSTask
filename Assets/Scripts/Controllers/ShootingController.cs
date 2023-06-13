using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controllers
{
    public class ShootingController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public Action OnStartAiming = null;
        public Action OnEndAiming = null;

        public void OnPointerDown(PointerEventData eventData)
        {
            OnStartAiming?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnEndAiming?.Invoke();
        }
    }
}