using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private Button _toGalleryButton;
        
        private void OnEnable()
        {
            _toGalleryButton.onClick.AddListener(OnGalleryClick);
        }

        private void OnGalleryClick()
        {
            
        }

        private void OnDisable()
        {
            _toGalleryButton.onClick.RemoveListener(OnGalleryClick);
        }
    }
}
